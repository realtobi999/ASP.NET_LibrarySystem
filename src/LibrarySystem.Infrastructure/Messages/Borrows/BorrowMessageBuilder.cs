using System.Net.Mail;
using LibrarySystem.Domain;
using LibrarySystem.Domain.Interfaces;
using Microsoft.Extensions.Configuration;

namespace LibrarySystem.Infrastructure.Messages.Borrows;

public class BorrowMessageBuilder(IConfiguration configuration) : MessageBuilder(configuration), IBorrowMessageBuilder
{
    public MailMessage BuildBookReturnMessage(ReturnBookMessageDto dto)
    {
        var message = BuildBaseMessage(dto.UserEmail);

        message.Subject = string.Format("{0} - Successfully returned!", dto.BookTitle);
        message.Body = AttachHtml("book_return_message.html", dto);

        return message;
    }
}
