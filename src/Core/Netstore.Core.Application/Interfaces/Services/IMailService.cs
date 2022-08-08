using Netstore.Core.Application.DTOs.Mail;
using System.Threading.Tasks;

namespace Netstore.Core.Application.Interfaces.Services;

public interface IMailService : ITransientService
{
    Task SendAsync(MailRequest request);
}