﻿namespace LibrarySystem.Domain.Extensions;

public class DirectoryExtensions
{
    public static string GetProjectSourceDirectory()
    {
        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        return Path.GetFullPath(Path.Combine(baseDirectory, "..", "..", "..", ".."));
    }
}
