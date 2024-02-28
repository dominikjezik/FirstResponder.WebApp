namespace FirstResponder.ApplicationCore.Incidents.DTOs;

public class IncidentItemDTO
{
    public Guid Id { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime? ResolvedAt { get; set; }
    
    public string Address { get; set; }
    
    public string? Patient { get; set; }
    
    public string Diagnosis { get; set; }
    
    public string State { get; set; }
    
    public string DisplayState { get; set; }
    
    public double? Latitude { get; set; }
    
    public double? Longitude { get; set; }
}