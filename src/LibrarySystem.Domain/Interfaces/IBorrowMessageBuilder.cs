using System.Net.Mail;

namespace LibrarySystem.Domain.Interfaces;

public interface IBorrowMessageBuilder
{
    MailMessage BuildBookReturnMessage(string toEmail, ReturnBookMessageDto dto);
}
