namespace FirstResponder.ApplicationCore.Incidents.DTOs;

public class IncidentResponderItemDTO
{
    public Guid ResponderId { get; set; }

    public string FullName { get; set; }

    public DateTime? AcceptedAt { get; set; }
    
    public double? Latitude { get; set; }
    
    public double? Longitude { get; set; }
    
    public string? TypeOfTransport { get; set; }
    
    public bool ReportSubmitted { get; set; }
}