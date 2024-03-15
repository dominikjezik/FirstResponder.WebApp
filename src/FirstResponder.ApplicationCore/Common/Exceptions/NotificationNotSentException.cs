namespace FirstResponder.ApplicationCore.Common.Exceptions;

public class NotificationNotSentException : Exception
{
    public NotificationNotSentException() : base()
    {
    }

    public NotificationNotSentException(string? message) : base(message)
    {
    }

    public NotificationNotSentException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}