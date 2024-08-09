using System.Net;
using LibrarySystem.Domain.Interfaces.Common;

namespace LibrarySystem.Domain.Exceptions.HTTP;

public class BadRequest400Exception : Exception, IHttpException
{
    public BadRequest400Exception(string message) : base(message)
    {
    }

    public int StatusCode => (int)HttpStatusCode.BadRequest;
    public string Title => "Bad Request";
}