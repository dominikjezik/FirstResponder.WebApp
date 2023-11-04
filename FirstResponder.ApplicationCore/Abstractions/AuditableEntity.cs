namespace FirstResponder.ApplicationCore.Abstractions;

public abstract class AuditableEntity<T> : BaseEntity<T>, IAuditable
{
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }
    
}