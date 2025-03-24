using System.Net.Mail;

namespace LibrarySystem.Domain.Interfaces.Emails;

public interface IEmailSender
{
    void SendEmail(MailMessage email);
}