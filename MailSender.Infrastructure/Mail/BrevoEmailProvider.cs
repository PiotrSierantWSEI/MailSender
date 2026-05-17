using System.Net;
using System.Net.Mail;
using MailSender.Application.Abstractions.Mail;
using MailSender.Infrastructure.Configuration;
using Microsoft.Extensions.Options;

namespace MailSender.Infrastructure.Mail;

public sealed class BrevoEmailProvider(IOptions<BrevoOptions> options) : IEmailProvider
{
    private readonly BrevoOptions _options = options.Value;

    public async Task<EmailSendResult> SendEmailAsync(EmailMessage emailMessage, CancellationToken cancellationToken)
    {
        try
        {
            // https://learn.microsoft.com/en-us/dotnet/api/system.net.mail.smtpclient.-ctor?view=netframework-4.8.1#system-net-mail-smtpclient-ctor
            using var smtpClient = new SmtpClient(_options.SmtpHost, _options.SmtpPort)
            {
                EnableSsl = _options.EnableSsl, // https://learn.microsoft.com/en-us/dotnet/api/system.net.mail.smtpclient.enablessl?view=netframework-4.8.1
                UseDefaultCredentials = false, // https://learn.microsoft.com/en-us/dotnet/api/system.net.mail.smtpclient.usedefaultcredentials?view=netframework-4.8.1
                Credentials = new NetworkCredential(_options.SmtpLogin, _options.SmtpPassword) // https://learn.microsoft.com/en-us/dotnet/api/system.net.mail.smtpclient.credentials?view=netframework-4.8.1
            };

            // https://learn.microsoft.com/en-us/dotnet/api/system.net.mail.mailaddress?view=netframework-4.8.1
            var fromAddress = string.IsNullOrWhiteSpace(emailMessage.From)
                ? new MailAddress(_options.SenderEmail, _options.SenderName)
                : new MailAddress(emailMessage.From);

            // https://learn.microsoft.com/en-us/dotnet/api/system.net.mail.mailmessage?view=netframework-4.8.1
            using var mailMessage = new MailMessage
            {
                From = fromAddress,
                Subject = emailMessage.Subject,
                Body = emailMessage.Body,
                IsBodyHtml = false
            };

            mailMessage.To.Add(emailMessage.To);
            await smtpClient.SendMailAsync(mailMessage, cancellationToken);

            return new EmailSendResult(true);
        }
        catch (Exception exception)
        {
            return new EmailSendResult(false, Error: exception.Message);
        }
    }
}
