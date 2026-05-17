namespace MailSender.Application.MailSend;

public sealed record MailSendResult(
    string AppId,
    string AppName,
    MailSendStatus Status,
    MailSendEmailObject Email
    );

public sealed record MailSendEmailObject(
    string To,
    string Subject,
    string Body
);

public enum MailSendStatus
{
    Success,
    Failure,
    Queued
}