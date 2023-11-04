using FirstResponder.ApplicationCore.Abstractions;

namespace FirstResponder.ApplicationCore.Entities.AedAggregate;

public class Model : BaseEntity<Guid>
{
    public required string Name { get; set; }
}
