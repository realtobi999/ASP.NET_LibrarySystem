using System.Net.Mail;

namespace LibrarySystem.Domain.Interfaces;

public interface IMessageBuilder
{
    MailMessage BuildBookReturnMessage(string toEmail, ReturnBookMessageDto dto);
}
