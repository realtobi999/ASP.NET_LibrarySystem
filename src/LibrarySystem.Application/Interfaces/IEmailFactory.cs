using LibrarySystem.Application.Interfaces.Emails;

namespace LibrarySystem.Application.Interfaces;

public interface IEmailFactory
{
    IBorrowEmailService CreateBorrowEmailService();
}
