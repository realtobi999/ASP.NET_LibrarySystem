namespace LibrarySystem.Application.Core.Extensions;

public static class DirectoryExtensions
{
    public static string GetProjectSourceDirectory()
    {
        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

        return Path.GetFullPath(Path.Combine(baseDirectory, "..", "..", "..", ".."));
    }
}