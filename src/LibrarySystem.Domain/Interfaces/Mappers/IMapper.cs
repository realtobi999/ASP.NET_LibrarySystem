namespace LibrarySystem.Domain.Interfaces.Mappers
{
    /// <summary>
    /// Provides mapping functionality for creating and updating entities from DTOs.
    /// </summary>
    /// <typeparam name="Entity">The entity type to be mapped.</typeparam>
    /// <typeparam name="CreateEntityDto">The DTO used for creating the entity.</typeparam>
    /// <typeparam name="UpdateEntityDto">The DTO used for updating the entity.</typeparam>
    public interface IMapper<Entity, CreateEntityDto, UpdateEntityDto>
        : ICreateMapper<Entity, CreateEntityDto>, IUpdateMapper<Entity, UpdateEntityDto>
    {
    }

    /// <summary>
    /// Provides functionality to create an entity from a DTO.
    /// </summary>
    /// <typeparam name="Entity">The entity type to be created.</typeparam>
    /// <typeparam name="CreateEntityDto">The DTO used for creating the entity.</typeparam>
    public interface ICreateMapper<Entity, CreateEntityDto>
    {
        /// <summary>
        /// Creates an entity from the specified DTO.
        /// </summary>
        /// <param name="dto">The DTO containing the data for the new entity.</param>
        /// <returns>The created entity.</returns>
        Entity CreateFromDto(CreateEntityDto dto);
    }

    /// <summary>
    /// Provides functionality to update an existing entity using a DTO.
    /// </summary>
    /// <typeparam name="Entity">The entity type to be updated.</typeparam>
    /// <typeparam name="UpdateEntityDto">The DTO used for updating the entity.</typeparam>
    public interface IUpdateMapper<Entity, UpdateEntityDto>
    {
        /// <summary>
        /// Updates the specified entity using the data from the DTO.
        /// </summary>
        /// <param name="entity">The entity to be updated.</param>
        /// <param name="dto">The DTO containing the updated data for the entity.</param>
        void UpdateFromDto(Entity entity, UpdateEntityDto dto);
    }
}
