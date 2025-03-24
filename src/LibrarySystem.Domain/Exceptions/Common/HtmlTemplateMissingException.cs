namespace LibrarySystem.Domain.Exceptions.Common;

public class HtmlTemplateMissingException : Exception
{
    public HtmlTemplateMissingException(string filePath) : base($"Html template was not found at '{filePath}', please create a new one or change the filepath.")
    {
    }
}