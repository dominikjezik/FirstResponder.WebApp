using FirstResponder.ApplicationCore.Common.Enums;

namespace FirstResponder.ApplicationCore.Aeds.DTOs;

public class AedItemFiltersDTO
{
    public AedGeneralType? Type { get; set; }
    
    public string? Holder { get; set; }
    
    public RegionOfState? Region { get; set; }
    
    public AedState? State { get; set; }
    
    public Guid? ManufacturerId { get; set; }
    
    public Guid? ModelId { get; set; }
    
    public string? SerialNumber { get; set; }
}