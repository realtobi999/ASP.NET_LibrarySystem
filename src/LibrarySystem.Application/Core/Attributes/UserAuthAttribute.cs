namespace LibrarySystem.Application.Core.Attributes;

// this is used by the UserAuthenticationMiddleware
[AttributeUsage(AttributeTargets.Method)]
public class UserAuthAttribute : Attribute
{
}