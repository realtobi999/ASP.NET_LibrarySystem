using LibrarySystem.Domain.Interfaces.Emails.Borrow;

namespace LibrarySystem.Domain.Interfaces.Managers;

public interface IEmailManager
{
    IBorrowEmailService Borrow { get; }
}