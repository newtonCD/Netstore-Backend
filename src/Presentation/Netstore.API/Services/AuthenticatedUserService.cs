using Microsoft.AspNetCore.Http;
using Netstore.Core.Application.Interfaces.Services;
using System.Security.Claims;

namespace Netstore.API.Services;

public class AuthenticatedUserService : IAuthenticatedUserService
{
    public AuthenticatedUserService(IHttpContextAccessor httpContextAccessor)
    {
        UserId = httpContextAccessor.HttpContext?.User?.FindFirstValue("uid");
    }

    public string UserId { get; }
    public string Username { get; }
}