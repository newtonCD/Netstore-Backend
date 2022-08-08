using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Netstore.Common.Results;
using Netstore.Core.Application.DTOs.Identity;
using Netstore.Core.Application.DTOs.Mail;
using Netstore.Core.Application.Enums;
using Netstore.Core.Application.Exceptions;
using Netstore.Core.Application.Interfaces.Services;
using Netstore.Core.Application.Settings;
using Netstore.Infrastructure.Identity.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Netstore.Infrastructure.Identity.Services;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly JwtSettings _jwtSettings;
    private readonly IDateTimeService _dateTimeService;
    private readonly IMailService _mailService;

    public IdentityService(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IOptions<JwtSettings> jwtSettings,
        IDateTimeService dateTimeService,
        SignInManager<ApplicationUser> signInManager,
        IMailService mailService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtSettings = jwtSettings.Value;
        _dateTimeService = dateTimeService;
        _signInManager = signInManager;
        _mailService = mailService;
    }

    public async Task<Result<TokenResponse>> GetTokenAsync(TokenRequest request, string ipAddress)
    {
        ApplicationUser user = await _userManager.FindByEmailAsync(request.Email);
        Guard.Against.Null(user, nameof(user), $"No Accounts Registered with {request.Email}.");
        SignInResult result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);
        Guard.Against.Default(user.EmailConfirmed, nameof(user.EmailConfirmed), $"Email is not confirmed for '{request.Email}'.");
        Guard.Against.Default(user.IsActive, nameof(user.IsActive), $"Account for '{request.Email}' is not active. Please contact the Administrator.");
        Guard.Against.Default(result.Succeeded, nameof(result.Succeeded), $"Invalid Credentials for '{request.Email}'.");

        JwtSecurityToken jwtSecurityToken = await GenerateJWToken(user, ipAddress);
        var response = new TokenResponse
        {
            Id = user.Id,
            JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
            IssuedOn = jwtSecurityToken.ValidFrom.ToLocalTime(),
            ExpiresOn = jwtSecurityToken.ValidTo.ToLocalTime(),
            Email = user.Email,
            UserName = user.UserName
        };

        IList<string> rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
        response.Roles = rolesList.ToList();
        response.IsVerified = user.EmailConfirmed;
        RefreshToken refreshToken = GenerateRefreshToken(ipAddress);
        response.RefreshToken = refreshToken.Token;
        return Result<TokenResponse>.Success(response, "Authenticated");
    }

    public async Task<Result<string>> RegisterAsync(RegisterRequest request, string origin)
    {
        ApplicationUser userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);
        if (userWithSameUserName != null)
        {
            throw new ApiException($"Username '{request.UserName}' is already taken.");
        }

        ApplicationUser user = new ApplicationUser
        {
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            UserName = request.UserName
        };

        ApplicationUser userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
        if (userWithSameEmail == null)
        {
            IdentityResult result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, nameof(Roles.Basic));
                string verificationUri = await SendVerificationEmail(user, origin);

                // TODO: Attach Email Service here and configure it via appsettings
                await _mailService.SendAsync(new MailRequest() { From = "newton.dantas@gmail.com", To = user.Email, Body = $"Please confirm your account by <a href='{verificationUri}'>clicking here</a>.", Subject = "Confirm Registration" });
                return Result<string>.Success(user.Id, message: $"User Registered. Confirmation Mail has been delivered to your Mailbox. (DEV) Please confirm your account by visiting this URL {verificationUri}");
            }
            else
            {
                throw new ApiException($"{result.Errors}");
            }
        }
        else
        {
            throw new ApiException($"Email {request.Email} is already registered.");
        }
    }

    public async Task<Result<string>> ConfirmEmailAsync(string userId, string code)
    {
        ApplicationUser user = await _userManager.FindByIdAsync(userId);
        code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        IdentityResult result = await _userManager.ConfirmEmailAsync(user, code);
        if (result.Succeeded)
        {
            return Result<string>.Success(user.Id, message: $"Account Confirmed for {user.Email}. You can now use the /api/identity/token endpoint to generate JWT.");
        }
        else
        {
            throw new ApiException($"An error occured while confirming {user.Email}.");
        }
    }

    public async Task ForgotPassword(ForgotPasswordRequest model, string origin)
    {
        ApplicationUser account = await _userManager.FindByEmailAsync(model.Email);

        // always return ok response to prevent email enumeration
        if (account == null) return;

        string code = await _userManager.GeneratePasswordResetTokenAsync(account);
#pragma warning disable S1481 // Unused local variables should be removed
        var enpointUri = new Uri(string.Concat($"{origin}/", "api/identity/reset-password/"));
        var emailRequest = new MailRequest()
        {
            Body = $"You reset token is - {code}",
            To = model.Email,
            Subject = "Reset Password",
        };
#pragma warning restore S1481 // Unused local variables should be removed

#pragma warning disable S125 // Sections of code should not be commented out

        // await _mailService.SendAsync(emailRequest);
#pragma warning restore S125 // Sections of code should not be commented out
    }

    public async Task<Result<string>> ResetPassword(ResetPasswordRequest model)
    {
        ApplicationUser account = await _userManager.FindByEmailAsync(model.Email);
        if (account == null) throw new ApiException($"No Accounts Registered with {model.Email}.");

        IdentityResult result = await _userManager.ResetPasswordAsync(account, model.Token, model.Password);
        if (result.Succeeded)
        {
            return Result<string>.Success(model.Email, message: "Password Resetted.");
        }
        else
        {
            throw new ApiException("Error occured while reseting the password.");
        }
    }

    private async Task<JwtSecurityToken> GenerateJWToken(ApplicationUser user, string ipAddress)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);
        var roleClaims = new List<Claim>();
        for (int i = 0; i < roles.Count; i++)
        {
            roleClaims.Add(new Claim("roles", roles[i]));
        }

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("uid", user.Id),
            new Claim("first_name", user.FirstName),
            new Claim("last_name", user.LastName),
            new Claim("full_name", $"{user.FirstName} {user.LastName}"),
            new Claim("ip", ipAddress)
        }
        .Union(userClaims)
        .Union(roleClaims);

        return JWTGeneration(claims);
    }

    private JwtSecurityToken JWTGeneration(IEnumerable<Claim> claims)
    {
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwtSettings.ValidIssuer,
            audience: _jwtSettings.ValidAudience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.TokenExpirationInMinutes),
            signingCredentials: signingCredentials);

        return jwtSecurityToken;
    }

    private static string RandomTokenString()
    {
        using RandomNumberGenerator rng = RandomNumberGenerator.Create();
        var uintBuffer = new byte[sizeof(uint)];
        rng.GetBytes(uintBuffer);
        return BitConverter.ToString(uintBuffer).Replace("-", string.Empty);
    }

    private RefreshToken GenerateRefreshToken(string ipAddress)
    {
        return new RefreshToken
        {
            Token = RandomTokenString(),
            Expires = DateTime.UtcNow.AddDays(7),
            Created = DateTime.UtcNow,
            CreatedByIp = ipAddress
        };
    }

    private async Task<string> SendVerificationEmail(ApplicationUser user, string origin)
    {
        string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        var enpointUri = new Uri(string.Concat($"{origin}/", "api/identity/confirm-email/"));
        string verificationUri = QueryHelpers.AddQueryString(enpointUri.ToString(), "userId", user.Id);
        verificationUri += QueryHelpers.AddQueryString(verificationUri, "code", code);

        // TODO: Email Service Call Here
        return verificationUri;
    }
}