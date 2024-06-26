using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Common.Enums;

namespace FirstResponder.ApplicationCore.Entities.AedAggregate;

public abstract class Aed : AuditableEntity<Guid>
{
    public AedState State { get; set; }

    public Guid? ManufacturerId { get; set; }
    public Manufacturer? Manufacturer { get; set; }
    
    public Guid? ModelId { get; set; }
    public Model? Model { get; set; }
    
    public Guid? LanguageId { get; set; }
    public Language? Language { get; set; }

    public string? SerialNumber { get; set; }

    public bool FullyAutomatic { get; set; }

    public bool ElectrodesAdults { get; set; }
    
    public bool ElectrodesChildren { get; set; }
    
    public DateTime? ElectrodesAdultsExpiration { get; set; }

    public DateTime? ElectrodesChildrenExpiration { get; set; }

    public DateTime? BatteryExpiration { get; set; }
    
    public string? Notes { get; set; }
    
    public abstract string GetDisplayHolder();
}