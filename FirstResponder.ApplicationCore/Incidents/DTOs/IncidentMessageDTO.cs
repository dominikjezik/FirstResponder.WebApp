namespace FirstResponder.ApplicationCore.Incidents.DTOs;

public class IncidentMessageDTO
{
    public Guid Id { get; set; }
    public string Content { get; set; }
    public string SenderName { get; set; }
    public DateTime CreatedAt { get; set; }
}