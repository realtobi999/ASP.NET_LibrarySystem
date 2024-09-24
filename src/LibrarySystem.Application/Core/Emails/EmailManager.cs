using LibrarySystem.Application.Interfaces;
using LibrarySystem.Domain.Interfaces.Emails.Borrow;
using LibrarySystem.Domain.Interfaces.Factories;

namespace LibrarySystem.Application.Core.Emails;

public class EmailManager : IEmailManager
{
    private readonly IEmailFactory _factory;

    public EmailManager(IEmailFactory factory)
    {
        _factory = factory;
    }

    public IBorrowEmailService Borrow => _factory.CreateBorrowEmailService();
}
