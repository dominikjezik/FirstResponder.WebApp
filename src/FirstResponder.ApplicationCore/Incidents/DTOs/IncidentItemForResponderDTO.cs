namespace FirstResponder.ApplicationCore.Incidents.DTOs;

public class IncidentItemForResponderDTO : IncidentItemDTO
{
    public DateTime? AcceptedAt { get; set; }
    
    public DateTime? DeclinedAt { get; set; }
}