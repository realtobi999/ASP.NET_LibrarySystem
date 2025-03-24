using LibrarySystem.Domain.Interfaces.Emails.Borrow;

namespace LibrarySystem.Domain.Interfaces.Factories;

public interface IEmailFactory
{
    IBorrowEmailService CreateBorrowEmailService();
}