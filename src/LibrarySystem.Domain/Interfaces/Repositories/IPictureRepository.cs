using System.Linq.Expressions;
using LibrarySystem.Domain.Entities;

namespace LibrarySystem.Domain.Interfaces.Repositories;

public interface IPictureRepository
{
    void Create(Picture picture);
    void Delete(Picture picture);
    void DeleteWhere(Expression<Func<Picture, bool>> expression);
}
