namespace LibrarySystem.Presentation.Extensions;

public static class EmployeeMiddlewareExtensions
{
    public static void UseEmployeeAuthentication(this IApplicationBuilder builder) =>
        builder.UseMiddleware<EmployeeAuthenticationMiddleware>();

}
