using FirstResponder.ApplicationCore.Abstractions;
using FirstResponder.ApplicationCore.Enums;

namespace FirstResponder.ApplicationCore.Entities;

public class User : AuditableEntity<Guid>
{
    public required string Email { get; set; }

    public required string FullName { get; set; }

    public required string PhoneNumber { get; set; }

    public DateTime DateOfBirth { get; set; }

    public string? Address { get; set; }

    public string? PostalCode { get; set; }

    public string? City { get; set; }

    public RegionOfState Region { get; set; }

    public string? Notes { get; set; }
    
    // TODO: Pridať rolu
    
}