using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using Netstore.Core.Application.DTOs.Mail;
using Netstore.Core.Application.Interfaces.Services;
using Netstore.Core.Application.Settings;
using System.Threading.Tasks;

namespace Netstore.Infrastructure.Services;

public class SmtpMailService : IMailService
{
    public SmtpMailService(IOptions<MailSettings> mailSettings, ILogger<SmtpMailService> logger)
    {
        MailSettings = mailSettings.Value;
        Logger = logger;
    }

    public MailSettings MailSettings { get; }
    public ILogger<SmtpMailService> Logger { get; }

    public async Task SendAsync(MailRequest request)
    {
        try
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(request.From ?? MailSettings.From);
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;
            var builder = new BodyBuilder();
            builder.HtmlBody = request.Body;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(MailSettings.Host, MailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(MailSettings.UserName, MailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
        catch (System.Exception ex)
        {
#pragma warning disable CA1848 // Use the LoggerMessage delegates
#pragma warning disable CA2254 // Template should be a static expression
            Logger.LogError(ex.Message, ex);
#pragma warning restore CA2254 // Template should be a static expression
#pragma warning restore CA1848 // Use the LoggerMessage delegates
        }
    }
}