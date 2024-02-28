namespace FirstResponder.ApplicationCore.Common.Exceptions;

public class EntityValidationException : Exception
{
    public Dictionary<string, string>? ValidationErrors { get; }

    public EntityValidationException(Dictionary<string, string>? validationErrors) : base("Entity validation failed")
    {
        ValidationErrors = validationErrors;
    }
    
    public EntityValidationException(string propertyName, string errorMessage) : base("Entity validation failed")
    {
        ValidationErrors = new Dictionary<string, string>
        {
            [propertyName] = errorMessage
        };
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