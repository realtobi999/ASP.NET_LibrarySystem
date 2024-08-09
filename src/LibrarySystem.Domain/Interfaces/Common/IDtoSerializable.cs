namespace LibrarySystem.Domain.Interfaces.Common;

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
