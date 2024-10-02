namespace LibrarySystem.Domain.Interfaces.Mappers;

public interface IMapper<Entity, Dto>
{
    Entity Map(Dto dto);
}
