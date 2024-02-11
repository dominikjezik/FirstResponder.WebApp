using FirstResponder.ApplicationCore.Common.Abstractions;

namespace FirstResponder.ApplicationCore.Entities.AedAggregate;

public class Manufacturer : BaseEntity<Guid>
{
    public required string Name { get; set; }
}
