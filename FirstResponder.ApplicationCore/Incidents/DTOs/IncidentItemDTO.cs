namespace FirstResponder.ApplicationCore.Incidents.DTOs;

public class IncidentItemDTO
{
    public Guid Id { get; set; }
    
    public string CreatedAt { get; set; }
    
    public string ResolvedAt { get; set; }
    
    public string Address { get; set; }
    
    public string? Patient { get; set; }
    
    public string Diagnosis { get; set; }
    
    public string State { get; set; }
    
    public double? Latitude { get; set; }
    
    public double? Longitude { get; set; }
}