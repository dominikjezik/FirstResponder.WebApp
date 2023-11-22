using FirstResponder.ApplicationCore.Abstractions;
using FirstResponder.ApplicationCore.Entities;
using FirstResponder.ApplicationCore.Enums;
using Microsoft.AspNetCore.Identity;

namespace FirstResponder.Infrastructure.Identity;

public class ApplicationUser : IdentityUser<Guid>, IAuditable
{
    public required string FullName { get; set; }

    public DateTime DateOfBirth { get; set; }

    public string? Address { get; set; }

    public string? PostalCode { get; set; }

    public string? City { get; set; }

    public RegionOfState Region { get; set; }

    public string? Notes { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }
}

public static class ApplicationUserExtensions
{
    public static User? ToDomainUser(this ApplicationUser? applicationUser)
    {
        if (applicationUser == null)
            return null;

        return new User
        {
            Id = applicationUser.Id,
            Email = applicationUser.Email,
            FullName = applicationUser.FullName,
            PhoneNumber = applicationUser.PhoneNumber,
            DateOfBirth = applicationUser.DateOfBirth,
            Address = applicationUser.Address,
            PostalCode = applicationUser.PostalCode,
            City = applicationUser.City,
            Region = applicationUser.Region,
            Notes = applicationUser.Notes,
            CreatedAt = applicationUser.CreatedAt,
            UpdatedAt = applicationUser.UpdatedAt
        };
    }
}

public static class UserExtensions
{
    public static ApplicationUser? ToApplicationUser(this User? user)
    {
        if (user == null)
            return null;

        return new ApplicationUser
        {
            Id = user.Id,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            DateOfBirth = user.DateOfBirth,
            Address = user.Address,
            PostalCode = user.PostalCode,
            City = user.City,
            Region = user.Region,
            Notes = user.Notes,
            FullName = user.FullName,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
    }
}