using LibrarySystem.Domain.Interfaces.Mappers;

namespace LibrarySystem.Domain.Interfaces.Factories;

public interface IMapperFactory
{
    IMapper<Entity, Dto> Initiate<Entity, Dto>();
}
