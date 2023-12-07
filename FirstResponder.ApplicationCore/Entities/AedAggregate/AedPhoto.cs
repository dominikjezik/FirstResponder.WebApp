using System.ComponentModel.DataAnnotations;
using FirstResponder.ApplicationCore.Abstractions;

namespace FirstResponder.ApplicationCore.Entities.AedAggregate;

public class AedPhoto : AuditableEntity<Guid>
{
    [Required]
    public Guid PublicAedId { get; set; }
    
    [Required]
    public string PhotoName { get; set; }
    
}