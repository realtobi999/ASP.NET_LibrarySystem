using LibrarySystem.Domain.Interfaces.Factories;
using LibrarySystem.Domain.Interfaces.Mappers;
using Microsoft.Extensions.DependencyInjection;

namespace LibrarySystem.Application.Core.Factories;

public class MapperFactory : IMapperFactory
{
    private readonly IServiceProvider _serviceProvider;

    public MapperFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IMapper<Entity, Dto> Initiate<Entity, Dto>()
    {
        var mapper = _serviceProvider.GetService<IMapper<Entity, Dto>>() ?? throw new Exception($"Mapper for types {typeof(Entity)} and {typeof(Dto)} not found.");
        
        return mapper;
    }
}

