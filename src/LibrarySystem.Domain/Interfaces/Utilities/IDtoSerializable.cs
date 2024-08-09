namespace LibrarySystem.Domain.Interfaces.Utilities;

public interface IDtoSerializable<out Dto> : IDtoSerializable
{
    new Dto ToDto();
}

public interface IDtoSerializable
{
    object ToDto()
    {
        return ((IDtoSerializable<object>)this).ToDto();
    }
}
