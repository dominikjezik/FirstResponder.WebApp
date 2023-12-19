using System.ComponentModel.DataAnnotations;
using FirstResponder.ApplicationCore.Abstractions;
using FirstResponder.ApplicationCore.Enums;

namespace FirstResponder.ApplicationCore.Entities.UserAggregate;

public class User : AuditableEntity<Guid>
{
    public required string Email { get; set; }

    public required string FullName { get; set; }

    public required string PhoneNumber { get; set; }

    [Required]
    public DateTime DateOfBirth { get; set; }

    public string? Address { get; set; }

    public string? PostalCode { get; set; }

    public string? City { get; set; }

    public RegionOfState Region { get; set; }

    public string? Notes { get; set; }

    public UserType Type { get; set; } = UserType.Default;
    
    public ICollection<Group> Groups { get; set; } = new List<Group>();
    
}