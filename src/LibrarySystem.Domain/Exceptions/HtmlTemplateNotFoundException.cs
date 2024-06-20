namespace LibrarySystem.Domain.Exceptions;

public class HtmlTemplateNotFoundException(string fileName) : InternalServerErrorException($"HTML template '{fileName}' not found at the specified path.")
{
}
