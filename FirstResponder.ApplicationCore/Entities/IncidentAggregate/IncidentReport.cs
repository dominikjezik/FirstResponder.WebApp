using FirstResponder.ApplicationCore.Common.Abstractions;

namespace FirstResponder.ApplicationCore.Entities.IncidentAggregate;

public class IncidentReport : AuditableEntity<Guid>
{
    public string? Details { get; set; }
    
    public bool AedUsed { get; set; }
    
    public int AedShocks { get; set; }
}