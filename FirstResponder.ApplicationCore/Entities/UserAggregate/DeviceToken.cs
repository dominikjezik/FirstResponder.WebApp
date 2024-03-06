using System.ComponentModel.DataAnnotations;
using FirstResponder.ApplicationCore.Common.Abstractions;

namespace FirstResponder.ApplicationCore.Entities.UserAggregate;

public class DeviceToken : AuditableEntity<Guid>
{
    [Required]
    public string Token { get; set; }
    
    public Guid UserId { get; set; }
    public User? User { get; set; }
}