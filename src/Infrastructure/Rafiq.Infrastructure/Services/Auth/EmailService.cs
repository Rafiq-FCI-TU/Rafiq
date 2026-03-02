using Microsoft.Extensions.Options;
using Rafiq.Application.Interfaces.Services;
using System.Net;
using System.Net.Mail;
using Rafiq.Domain.Settings;

namespace Rafiq.Infrastructure.Services.Auth;
public class EmailService(IOptions<EmailSettings> emailSettings) : IEmailService
{
    private readonly EmailSettings _settings = emailSettings.Value;

    public async Task SendAsync(string toEmail, string subject, string body)
    {
        using var smtpClient = new SmtpClient(_settings.Host, _settings.Port);
        smtpClient.Credentials = new NetworkCredential(_settings.SenderEmail, _settings.Password);
        smtpClient.EnableSsl = _settings.EnableSsl;

        var mailMessage = new MailMessage
        {
            From = new MailAddress(_settings.SenderEmail, _settings.SenderName),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        mailMessage.To.Add(toEmail);

        await smtpClient.SendMailAsync(mailMessage);
    }
}