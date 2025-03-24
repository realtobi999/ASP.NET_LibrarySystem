namespace LibrarySystem.Domain.Interfaces.Common;

/// <summary>
/// Provides serialization functionality for converting an object to a specific DTO.
/// </summary>
/// <typeparam name="TDto">The type of the DTO to be serialized.</typeparam>
public interface IDtoSerialization<out TDto> : IDtoSerialization
{
    /// <summary>
    /// Serializes the current object to its corresponding DTO.
    /// </summary>
    /// <returns>The DTO representation of the current object.</returns>
    new TDto ToDto();
}

/// <summary>
/// Provides general serialization functionality for converting an object to a DTO.
/// </summary>
public interface IDtoSerialization
{
    /// <summary>
    /// Serializes the current object to its corresponding DTO.
    /// </summary>
    /// <returns>The DTO as an object.</returns>
    object ToDto()
    {
        return ((IDtoSerialization<object>)this).ToDto();
    }
}