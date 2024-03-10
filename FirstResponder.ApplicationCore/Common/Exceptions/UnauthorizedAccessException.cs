namespace FirstResponder.ApplicationCore.Common.Exceptions;

public class UnauthorizedAccessException : Exception
{
    public UnauthorizedAccessException() : base()
    {
    }

    public UnauthorizedAccessException(string? message) : base(message)
    {
    }

    public UnauthorizedAccessException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}