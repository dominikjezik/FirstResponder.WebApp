using FirstResponder.ApplicationCore.Common.Abstractions;

namespace FirstResponder.ApplicationCore.Entities.AedAggregate;

public class Language : BaseEntity<Guid>
{
    public required string Name { get; set; }
}
