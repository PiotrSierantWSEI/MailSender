namespace MailSender.Contracts.MailSend;

public sealed record MailSendRequest(
    string To,
    string Subject,
    string Body);
