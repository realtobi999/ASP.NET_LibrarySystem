using LibrarySystem.Application.Interfaces;
using LibrarySystem.Application.Interfaces.Emails;

namespace LibrarySystem.Application.Emails;

public class EmailManager : IEmailManager
{
    private readonly IEmailFactory _factory;

    public EmailManager(IEmailFactory factory)
    {
        _factory = factory;
    }

    public IBorrowEmailService Borrow => _factory.CreateBorrowEmailService();
}
