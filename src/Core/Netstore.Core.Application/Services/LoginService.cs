using Netstore.Core.Application.Interfaces.Services;
using System.Threading.Tasks;

namespace Netstore.Core.Application.Services;

public class LoginService : ILoginService
{
    public async Task<bool> IsValidUserNameAndPasswordAsync(string userName, string password)
    {
        // TODO: implementar validação de usuário e senha.
        return await Task.FromResult(userName.Equals("Newton", System.StringComparison.InvariantCultureIgnoreCase)
                                     && password == "123456");
    }
}