using System.Net;
using LibrarySystem.Domain.Interfaces.Common;

namespace LibrarySystem.Domain.Exceptions.HTTP;

public class Conflict409Exception : Exception, IHttpException
{
    public Conflict409Exception(string? message) : base(message)
    {
    }

    public int StatusCode => (int)HttpStatusCode.Conflict;
    public string Title => "Conflict";
}