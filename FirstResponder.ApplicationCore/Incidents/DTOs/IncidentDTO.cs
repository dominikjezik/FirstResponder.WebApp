using FirstResponder.ApplicationCore.Common.Enums;

namespace FirstResponder.ApplicationCore.Incidents.DTOs;

public class IncidentDTO
{
    public Guid IncidentId { get; set; }
    
    public IncidentFormDTO IncidentForm { get; set; }

    public IncidentState State { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime? ResolvedAt { get; set; }
    
    public List<ResponderItemDTO> Responders { get; set; } = new();
    
    public class ResponderItemDTO
    {
        public Guid ResponderId { get; set; }
        
        public string FullName { get; set; }
        
        public DateTime? AcceptedAt { get; set; }
    }
}