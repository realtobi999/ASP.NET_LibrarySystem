namespace LibrarySystem.Domain.Interfaces.Common;

public interface IHttpException
{
    public int StatusCode { get; }
    public string Title { get; }
    public string Message { get; }
}
