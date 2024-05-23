using System.Text.Json;

namespace LibrarySystem.Domain.Entities;

public class ErrorMessage
{
    public int StatusCode { get; set; }
    public string? Message { get; set; }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}
