using System.Net.Mail;
using LibrarySystem.Domain.Interfaces.Emails;

namespace LibrarySystem.EmailService;

public class EmailSender : IEmailSender
{
    private readonly SmtpClient _client;

    public EmailSender(SmtpClient client)
    {
        _client = client;
    }

    public void SendEmail(MailMessage email)
    {
        _client.Send(email);
    }
}