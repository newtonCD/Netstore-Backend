using Netstore.Core.Application.Services.Authentication;
using System.Threading.Tasks;

namespace Netstore.Core.Application.Interfaces.Services.Authentication;

public interface IAuthenticationService : ITransientService
{
    AuthenticationResult Login(string email, string password);
    AuthenticationResult Register(string firstName, string lastName, string email, string password);
    Task<AuthenticationResult> RegisterAsync(string firstName, string lastName, string email, string password);
}
