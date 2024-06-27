using LibrarySystem.Domain;

namespace LibrarySystem.Application.Interfaces.Emails;

public interface IBorrowEmailService
{
    void SendReturnBookEmail(ReturnBookMessageDto messageDto);
}
