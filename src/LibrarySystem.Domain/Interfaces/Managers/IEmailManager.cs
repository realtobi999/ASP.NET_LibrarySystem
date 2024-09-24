using LibrarySystem.Domain.Interfaces.Emails.Borrow;

namespace LibrarySystem.Application.Interfaces;

public interface IEmailManager
{
    IBorrowEmailService Borrow { get; }
}
