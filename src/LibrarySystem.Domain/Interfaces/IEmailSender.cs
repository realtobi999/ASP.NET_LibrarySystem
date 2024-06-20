using System.Net.Mail;

namespace LibrarySystem.Domain.Interfaces;

public interface IEmailSender
{
    void SendEmail(MailMessage email);
}
