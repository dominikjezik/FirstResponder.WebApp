using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Entities.UserAggregate;

namespace FirstResponder.ApplicationCore.Entities.IncidentAggregate;

public class IncidentMessage : AuditableEntity<Guid>
{
    public string Content { get; set; }
    
    public Guid IncidentId { get; set; }
    public Incident? Incident { get; set; }
    
    public Guid SenderId { get; set; }
    public User? Sender { get; set; }
}