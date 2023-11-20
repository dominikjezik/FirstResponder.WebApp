using FirstResponder.ApplicationCore.Entities;
using FirstResponder.ApplicationCore.Enums;

namespace FirstResponder.ApplicationCore.Users.DTOs;

public class UserFormDTO
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
    
    public User ToUser()
    {
        return new User
        {
            Email = Email,
            FullName = FullName,
            PhoneNumber = PhoneNumber,
            DateOfBirth = DateOfBirth,
            Address = Address,
            PostalCode = PostalCode,
            City = City,
            Region = Region,
            Notes = Notes
        };
    }
}

public static class UserExtensions
{
    public static UserFormDTO ToUserFormDTO(this User user)
    {
        return new UserFormDTO
        {
            Email = user.Email,
            FullName = user.FullName,
            PhoneNumber = user.PhoneNumber,
            DateOfBirth = user.DateOfBirth,
            Address = user.Address,
            PostalCode = user.PostalCode,
            City = user.City,
            Region = user.Region,
            Notes = user.Notes
        };
    }
}