namespace Netstore.Core.Application.Interfaces.Services;

public interface IAuthenticatedUserService : ITransientService
{
    string UserId { get; }
    public string Username { get; }
}