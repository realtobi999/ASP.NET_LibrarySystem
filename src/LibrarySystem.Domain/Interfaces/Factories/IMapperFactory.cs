using LibrarySystem.Domain.Interfaces.Common;

namespace LibrarySystem.Domain.Interfaces.Factories;

public interface IMapperFactory
{
    IMapper<TEntity, TDto> Initiate<TEntity, TDto>();
}