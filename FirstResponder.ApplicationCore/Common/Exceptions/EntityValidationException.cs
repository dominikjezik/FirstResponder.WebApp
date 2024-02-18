namespace FirstResponder.ApplicationCore.Common.Exceptions;

public class EntityValidationException : Exception
{
    public Dictionary<string, string>? ValidationErrors { get; }

    public EntityValidationException(Dictionary<string, string>? validationErrors) : base("Entity validation failed")
    {
        ValidationErrors = validationErrors;
    }
    
    public EntityValidationException() : base()
    {
    }

    public EntityValidationException(string? message) : base(message)
    {
    }

    public EntityValidationException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}