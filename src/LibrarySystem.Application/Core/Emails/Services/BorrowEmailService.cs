using LibrarySystem.Domain.Dtos.Email.Messages;
using LibrarySystem.Domain.Interfaces.Emails;
using LibrarySystem.Domain.Interfaces.Emails.Borrow;

namespace LibrarySystem.Application.Core.Emails.Services;

public class BorrowEmailService : IBorrowEmailService
{
    private readonly IBorrowMessageBuilder _builder;
    private readonly IEmailSender _sender;

    public BorrowEmailService(IBorrowMessageBuilder builder, IEmailSender sender)
    {
        _builder = builder;
        _sender = sender;
    }

    public void SendBorrowBookEmail(BorrowBookMessageDto messageDto)
    {
        var message = _builder.BuildBorrowBookMessage(messageDto);

        _sender.SendEmail(message);
    }

    public void SendReturnBookEmail(ReturnBookMessageDto messageDto)
    {
        var message = _builder.BuildReturnBookMessage(messageDto);

        _sender.SendEmail(message);
    }
}
