using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Netstore.Core.Application.Interfaces.Services;
using Netstore.Core.Application.Models;
using Netstore.Core.Application.Settings;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Netstore.API.Controllers.V1;

[ApiVersion("1.0")]
public class AuthenticationController : ApiControllerBase
{
    private readonly IConfiguration _config;
    private readonly ILoginService _loginService;

    public AuthenticationController(IConfiguration config, ILoginService loginService)
    {
        _config = config;
        _loginService = loginService;
    }

    /// <summary>
    /// Login de usuário para obtenção de token.
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> LoginAsync([FromBody] Login login)
    {
        if (login is null)
        {
            return BadRequest("Invalid login request!!!");
        }

        bool isValidLogin = await _loginService.IsValidUserNameAndPasswordAsync(login.UserName, login.Password);

        if (isValidLogin)
        {
            JwtSettings jwtSettings = _config.GetSection(nameof(JwtSettings)).Get<JwtSettings>();

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokeOptions = new JwtSecurityToken(
                                                    issuer: jwtSettings.ValidIssuer,
                                                    audience: jwtSettings.ValidAudience,
                                                    claims: new List<Claim>(),
                                                    expires: DateTime.Now.AddMinutes(jwtSettings.TokenExpirationInMinutes),
                                                    signingCredentials: signinCredentials);

            string tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

            return Ok(new JwtTokenResponse { Token = tokenString });
        }

        return Unauthorized();
    }
}