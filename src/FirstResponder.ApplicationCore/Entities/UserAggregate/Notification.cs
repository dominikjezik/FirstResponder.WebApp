using System.ComponentModel.DataAnnotations;
using FirstResponder.ApplicationCore.Common.Abstractions;

namespace FirstResponder.ApplicationCore.Entities.UserAggregate;

public class Notification : AuditableEntity<Guid>
{
    [Required]
    public string Content { get; set; }
    
    public Guid SenderId { get; set; }
    public User? Sender { get; set; }
    
    public ICollection<NotificationUser> Recipients { get; set; }
}