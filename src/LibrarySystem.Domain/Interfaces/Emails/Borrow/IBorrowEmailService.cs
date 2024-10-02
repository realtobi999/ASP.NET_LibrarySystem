using LibrarySystem.Domain.Dtos.Email.Messages;

namespace LibrarySystem.Domain.Interfaces.Emails.Borrow;

public interface IBorrowEmailService
{
    void SendReturnBookEmail(ReturnBookMessageDto messageDto);
    void SendBorrowBookEmail(BorrowBookMessageDto messageDto);
}
