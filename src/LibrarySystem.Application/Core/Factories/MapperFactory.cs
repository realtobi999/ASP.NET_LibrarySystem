using LibrarySystem.Domain.Interfaces.Factories;
using LibrarySystem.Domain.Interfaces.Common;
using Microsoft.Extensions.DependencyInjection;

namespace LibrarySystem.Application.Core.Factories;

public class MapperFactory : IMapperFactory
{
    private readonly IServiceProvider _serviceProvider;

    public MapperFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IMapper<TEntity, TDto> Initiate<TEntity, TDto>()
    {
        var mapper = _serviceProvider.GetService<IMapper<TEntity, TDto>>() ?? throw new Exception($"Mapper for types {typeof(TEntity)} and {typeof(TDto)} not found.");

        return mapper;
    }
}