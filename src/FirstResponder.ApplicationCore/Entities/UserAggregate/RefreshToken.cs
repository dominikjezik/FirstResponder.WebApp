using System.ComponentModel.DataAnnotations;
using FirstResponder.ApplicationCore.Common.Abstractions;

namespace FirstResponder.ApplicationCore.Entities.UserAggregate;

public class RefreshToken : BaseEntity<Guid>
{
    [Required]
    public string Token { get; set; }
    
    public DateTime Expires { get; set; }
    
    public Guid UserId { get; set; }
}