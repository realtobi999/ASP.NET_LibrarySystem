﻿using System.Net.Mail;
using LibrarySystem.Domain.Dtos.Email.Messages;
using LibrarySystem.Domain.Interfaces.Emails.Borrow;
using Microsoft.Extensions.Configuration;

namespace LibrarySystem.Infrastructure.Messages.Builders;

public class BorrowMessageBuilder(IConfiguration configuration) : MessageBuilderBase(configuration), IBorrowMessageBuilder
{
    public MailMessage BuildBorrowBookMessage(BorrowBookMessageDto dto)
    {
        var message = BuildBaseMessage(dto.UserEmail);

        message.Subject = $"{dto.BookTitle} - Successfully borrowed!";
        message.Body = AttachHtml("book_borrow_message.html", dto);

        return message;
    }

    public MailMessage BuildReturnBookMessage(ReturnBookMessageDto dto)
    {
        var message = BuildBaseMessage(dto.UserEmail);

        message.Subject = $"{dto.BookTitle} - Successfully returned!";
        message.Body = AttachHtml("book_return_message.html", dto);

        return message;
    }
}