using System.ComponentModel.DataAnnotations;
using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Common.Enums;

namespace FirstResponder.ApplicationCore.Entities.IncidentAggregate;

public class Incident : AuditableEntity<Guid>
{
    public IncidentState State { get; set; } = IncidentState.Created;
    
    public DateTime? ResolvedAt { get; set; }
    
    [Required]
    public string Address { get; set; }
    
    public string? Patient { get; set; }
    
    [Required]
    public string Diagnosis { get; set; }
    
    [Required]
    [Range(-90, 90)]
    public double Latitude { get; set; }
    
    [Required]
    [Range(-180, 180)]
    public double Longitude { get; set; }
    
    public List<IncidentResponder> Responders { get; set; } = new();
    
}