using LibrarySystem.Domain.Dtos.Messages;

namespace LibrarySystem.Application.Interfaces.Emails;

public interface IBorrowEmailService
{
    void SendReturnBookEmail(ReturnBookMessageDto messageDto);
    void SendBorrowBookEmail(BorrowBookMessageDto messageDto);
}
