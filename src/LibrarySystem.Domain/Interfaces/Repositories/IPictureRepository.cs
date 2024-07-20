using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Interfaces.Repositories;

public interface IPictureRepository
{
    void Create(Picture picture);
    void Delete(Picture picture);
}
