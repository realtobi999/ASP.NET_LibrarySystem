namespace LibrarySystem.Domain.Exceptions.NotFound;

public class HtmlTemplateNotFoundException(string fileName) : InternalServerErrorException($"HTML template '{fileName}' not found at the specified path.")
{
}
