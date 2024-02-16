using FirstResponder.ApplicationCore.Entities.UserAggregate;

namespace FirstResponder.ApplicationCore.Entities.IncidentAggregate;

public class IncidentResponder
{
    public Guid IncidentId { get; set; }
    public Incident? Incident { get; set; }
	
    public Guid ResponderId { get; set; }
    public User? Responder { get; set; }
    
    public DateTime? AcceptedAt { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public bool IsDeclined { get; set; } = false;
    
}