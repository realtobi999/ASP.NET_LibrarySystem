using System.Net.Mail;
using LibrarySystem.Domain.Dtos.Messages;

namespace LibrarySystem.Domain.Interfaces;

public interface IBorrowMessageBuilder
{
    MailMessage BuildReturnBookMessage(ReturnBookMessageDto dto);
    MailMessage BuildBorrowBookMessage(BorrowBookMessageDto dto);
}
