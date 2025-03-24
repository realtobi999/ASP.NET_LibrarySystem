namespace LibrarySystem.Domain.Interfaces.Common;

public interface IMapper<out TEntity, in TDto>
{
    TEntity Map(TDto dto);
}