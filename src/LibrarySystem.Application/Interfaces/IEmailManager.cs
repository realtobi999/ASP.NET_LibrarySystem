using LibrarySystem.Application.Interfaces.Emails;

namespace LibrarySystem.Application.Interfaces;

public interface IEmailManager
{
    IBorrowEmailService Borrow { get; }
}
