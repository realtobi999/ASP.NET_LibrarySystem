using LibrarySystem.Presentation.Middlewares;

namespace LibrarySystem.Presentation.Extensions;

public static class UserMiddlewareExtensions
{
    public static void UseUserAuthentication(this IApplicationBuilder builder) =>
        builder.UseMiddleware<UserAuthenticationMiddleware>();
}
