using System.Net.Mail;
using LibrarySystem.Domain.Dtos.Messages;
using LibrarySystem.Domain.Interfaces.Emails;
using Microsoft.Extensions.Configuration;

namespace LibrarySystem.Infrastructure.Messages.Borrows;

public class BorrowMessageBuilder(IConfiguration configuration) : MessageBuilder(configuration), IBorrowMessageBuilder
{
    public MailMessage BuildBorrowBookMessage(BorrowBookMessageDto dto)
    {
        var message = BuildBaseMessage(dto.UserEmail);

        message.Subject = string.Format("{0} - Successfully borrowed!", dto.BookTitle);
        message.Body = AttachHtml("book_borrow_message.html", dto);

        return message;
    }

    public MailMessage BuildReturnBookMessage(ReturnBookMessageDto dto)
    {
        var message = BuildBaseMessage(dto.UserEmail);

        message.Subject = string.Format("{0} - Successfully returned!", dto.BookTitle);
        message.Body = AttachHtml("book_return_message.html", dto);

        return message;
    }
}
