using LibrarySystem.Application.Emails;
using LibrarySystem.Application.Interfaces;
using LibrarySystem.Application.Interfaces.Emails;
using LibrarySystem.Domain.Interfaces;

namespace LibrarySystem.Application.Factories;

public class EmailFactory : IEmailFactory
{
    private readonly IBorrowMessageBuilder _borrowBuilder;
    private readonly IEmailSender _sender;

    public EmailFactory(IBorrowMessageBuilder borrowBuilder, IEmailSender sender)
    {
        _borrowBuilder = borrowBuilder;
        _sender = sender;
    }

    public IBorrowEmailService CreateBorrowEmailService()
    {
        return new BorrowEmailService(_borrowBuilder, _sender);
    }
}
