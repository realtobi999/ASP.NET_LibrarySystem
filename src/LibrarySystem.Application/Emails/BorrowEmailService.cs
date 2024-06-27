using LibrarySystem.Application.Interfaces.Emails;
using LibrarySystem.Domain;
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

    public void SendReturnBookEmail(ReturnBookMessageDto messageDto)
    {
         var message = _builder.BuildBookReturnMessage(messageDto);

        _sender.SendEmail(message);
    }
}
