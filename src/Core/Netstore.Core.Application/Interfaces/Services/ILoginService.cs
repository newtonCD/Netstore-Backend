using System.Threading.Tasks;

namespace Netstore.Core.Application.Interfaces.Services;

public interface ILoginService : ITransientService
{
    Task<bool> IsValidUserNameAndPasswordAsync(string userName, string password);
}