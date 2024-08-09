using System.Net;
using LibrarySystem.Domain.Interfaces.Common;

namespace LibrarySystem.Domain.Exceptions.HTTP;

public class NotFound404Exception : Exception, IHttpException
{
    public NotFound404Exception(string entity, object key) : base($"The requested {entity} with the key '{key}' was not found in the system.")
    {
    }

    public NotFound404Exception(string entity, params object[] keys) : base($"The requested {entity} with the keys '{string.Join(", ", keys)}' were not found in the system.")
    {
    }

    public int StatusCode => (int)HttpStatusCode.NotFound;
    public string Title => "Resource Not Found";
}