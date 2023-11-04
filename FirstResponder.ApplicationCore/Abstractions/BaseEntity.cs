namespace FirstResponder.ApplicationCore.Abstractions;

public abstract class BaseEntity<T>
{
    public T Id { get; set; }
}
