using LibrarySystem.Domain.Dtos.Messages;

namespace LibrarySystem.Domain.Interfaces.Emails.Borrow;

public interface IBorrowEmailService
{
    void SendReturnBookEmail(ReturnBookMessageDto messageDto);
    void SendBorrowBookEmail(BorrowBookMessageDto messageDto);
}
