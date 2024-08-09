using System.Net;
using LibrarySystem.Domain.Interfaces.Common;

namespace LibrarySystem.Domain.Exceptions.HTTP;

public class ServiceUnavailable503Exception : Exception, IHttpException
{
    public ServiceUnavailable503Exception(string message) : base(message)
    {
    }

    public int StatusCode => (int)HttpStatusCode.ServiceUnavailable;
    public string Title => "Service Unavailable";
}
