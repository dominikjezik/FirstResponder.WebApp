namespace FirstResponder.ApplicationCore.Incidents.DTOs;

public class IncidentReportDTO
{
    public Guid IncidentId { get; set; }
    
    public Guid ResponderId { get; set; }
    
    public string? SubmittedAt { get; set; }
    
    public string Responder { get; set; }
    
    public IncidentReportFormDTO IncidentReportForm { get; set; }
}