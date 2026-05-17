using MailSender.Application.Abstractions.Mail;
using MailSender.Application.Abstractions.SharedConstants;

namespace MailSender.Application.MailSend;

public sealed class MailSendAppUseCase(IEmailProvider emailProvider)
{
    private readonly IEmailProvider _emailProvider = emailProvider;

    public async Task<MailSendResult> ExecuteAsync(
        MailSendCommand command,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(command.AppId) ||
            string.IsNullOrWhiteSpace(command.AppName) ||
            string.IsNullOrWhiteSpace(command.To) ||
            string.IsNullOrWhiteSpace(command.Subject) ||
            string.IsNullOrWhiteSpace(command.Body))
        {
            throw new ArgumentException("Invalid command parameters");
        }

        /* 
            WYMAGANIA BIZNESOWE: 
            - subject konczy sie '?' przed wyslaniem maila dodajemy prefix '[Q]'
            - jesli body zawiera moje nazwisko dodaj prefix [student.surname] przed nazwiskiem i [/student.surname] za nazwiskiem.
        */
        var subject = command.Subject;
        var body = command.Body;

        if (command.Subject.EndsWith("?"))
        {
            subject = "[Q] " + command.Subject;
        }

        if (command.Body.Contains(SharedConstants.MySurname, StringComparison.OrdinalIgnoreCase))
        {
            // Replace zmienia wszystkie wystapienia w stringu, nie patrzymy na wielkosc liter.
            body = command.Body.Replace(
                SharedConstants.MySurname,
                $"[student.surname] {SharedConstants.MySurname} [/student.surname]",
                StringComparison.OrdinalIgnoreCase);
        }

        var sendResult = await _emailProvider.SendEmailAsync(
            new EmailMessage(command.To, subject, body),
            cancellationToken);

        return new MailSendResult(
            command.AppId,
            command.AppName,
            sendResult.Success ? MailSendStatus.Success : MailSendStatus.Failure,
            new MailSendEmailObject(command.To, subject, body),
            sendResult.Error);
    }
}
