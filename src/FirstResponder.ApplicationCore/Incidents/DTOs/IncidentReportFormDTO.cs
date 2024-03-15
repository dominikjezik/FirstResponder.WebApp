namespace FirstResponder.ApplicationCore.Incidents.DTOs;

public class IncidentReportFormDTO
{
    public string? Details { get; set; }
    
    public bool AedUsed { get; set; }
    
    public int AedShocks { get; set; }
}