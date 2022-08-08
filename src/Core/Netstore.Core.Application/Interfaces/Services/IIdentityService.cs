using Netstore.Common.Results;
using Netstore.Core.Application.DTOs.Identity;
using System.Threading.Tasks;

namespace Netstore.Core.Application.Interfaces.Services;

public interface IIdentityService : ITransientService
{
    Task<Result<TokenResponse>> GetTokenAsync(TokenRequest request, string ipAddress);
    Task<Result<string>> RegisterAsync(RegisterRequest request, string origin);
    Task<Result<string>> ConfirmEmailAsync(string userId, string code);
    Task ForgotPassword(ForgotPasswordRequest model, string origin);
    Task<Result<string>> ResetPassword(ResetPasswordRequest model);
}