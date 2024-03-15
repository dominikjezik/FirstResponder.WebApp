using FirstResponder.ApplicationCore.Common.Abstractions;

namespace FirstResponder.ApplicationCore.Entities.AedAggregate;

public class Model : BaseEntity<Guid>
{
    public required string Name { get; set; }
    
    public Guid? ManufacturerId { get; set; }
    public Manufacturer? Manufacturer { get; set; }
}