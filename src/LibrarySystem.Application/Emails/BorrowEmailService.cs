using LibrarySystem.Application.Interfaces.Emails;
using LibrarySystem.Domain;
using LibrarySystem.Domain.Dtos.Messages;
using LibrarySystem.Domain.Interfaces;

namespace LibrarySystem.Application.Emails;

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
