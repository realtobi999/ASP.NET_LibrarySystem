using System.Net.Mail;
using LibrarySystem.Domain.Dtos.Messages;

namespace LibrarySystem.Domain.Interfaces.Emails.Borrow;

public interface IBorrowMessageBuilder
{
    MailMessage BuildReturnBookMessage(ReturnBookMessageDto dto);
    MailMessage BuildBorrowBookMessage(BorrowBookMessageDto dto);
}
