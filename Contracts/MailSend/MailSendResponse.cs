namespace MailSender.Contracts.MailSend;

public sealed record MailSendResponse(
    string AppId,
    string AppName,
    string Status,
    MailSendEmailObject Email
    );

public sealed record MailSendEmailObject(
    string To,
    string Subject,
    string Body
);