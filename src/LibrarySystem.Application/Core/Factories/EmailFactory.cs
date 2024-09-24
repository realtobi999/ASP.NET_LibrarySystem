using LibrarySystem.Application.Core.Emails;
using LibrarySystem.Domain.Interfaces.Emails;
using LibrarySystem.Domain.Interfaces.Emails.Borrow;
using LibrarySystem.Domain.Interfaces.Factories;

namespace LibrarySystem.Application.Core.Factories;

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
