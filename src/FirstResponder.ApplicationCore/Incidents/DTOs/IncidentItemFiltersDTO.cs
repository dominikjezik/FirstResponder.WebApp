using FirstResponder.ApplicationCore.Common.Enums;

namespace FirstResponder.ApplicationCore.Incidents.DTOs;

public class IncidentItemFiltersDTO
{
    public DateTime? From { get; set; }
    
    public DateTime? To { get; set; }
    
    public string? Patient { get; set; }
    
    public string? Address { get; set; }
    
    public IncidentState? State { get; set; }
}