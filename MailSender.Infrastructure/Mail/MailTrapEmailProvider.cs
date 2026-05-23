using System;
using System.Net;
using System.Net.Mail;
using MailSender.Application.Abstractions.Mail;
using MailSender.Infrastructure.Configuration;
using Microsoft.Extensions.Options;

namespace MailSender.Infrastructure.Mail;

// wydaje mi sie ze mozna zrobic jeden provider do wysylania maili, a konfiguracje do niego wstrzykiwac z appsettingsa i zaleznosci a nie robic osobnych providerow do kazdego serwisu, w tym przypadku zrobilem osobny provider do mailtrapa - moze warto zrefaktoryzowac.

public sealed class MailTrapEmailProvider(IOptions<MailTrapOptions> options) : IEmailProvider
{
    private readonly MailTrapOptions _options = options.Value;

    public async Task<EmailSendResult> SendEmailAsync(EmailMessage emailMessage, CancellationToken cancellationToken)
    {
        try
        {
            // https://mailtrap.io/blog/csharp-send-email/

            // korzystamy z darmowej wersji i warunek jest taki że: 
            // - moge wyslac email tylko do swojego konta
            // - uzyc domeny demomailtrap.co

            /* 
                This domain can only be used to send emails to yourself.
                To send emails to others, add and verify your own domain.
            */

            var fromAddress = string.IsNullOrWhiteSpace(emailMessage.From)
              ? new MailAddress(_options.SenderEmail, _options.SenderName)
              : new MailAddress(emailMessage.From);

            MailAddress toAddress = new MailAddress(emailMessage.To);

            MailMessage email = new MailMessage(fromAddress, toAddress)
            {
                Subject = emailMessage.Subject,
                Body = emailMessage.Body,
            };

            SmtpClient smtp = new()
            {
                Host = _options.SmtpHost,
                Port = _options.SmtpPort,
                Credentials = new NetworkCredential(_options.SmtpLogin, _options.SmtpPassword),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = _options.EnableSsl
            };

            /* Send method called below is what will send off our email 
            * unless an exception is thrown.
            */
            await smtp.SendMailAsync(email, cancellationToken);
            return new EmailSendResult(true);
        }
        catch (SmtpException exception)
        {
            Console.WriteLine("An error occurred while sending email: {0}", exception.ToString());
            return new EmailSendResult(false, Error: exception.ToString());
        }
    }
}
