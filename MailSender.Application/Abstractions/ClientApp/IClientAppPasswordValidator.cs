namespace MailSender.Application.Abstractions.ClientApp;

// Interfejs definiujący walidator hasła aplikacji klienta
public interface IClientAppPasswordValidator
{
    (bool IsValid, int PasswordIndex) ValidatePassword(string password);
}
