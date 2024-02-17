using System.ComponentModel.DataAnnotations;
using FirstResponder.ApplicationCore.Common.Enums;
using FirstResponder.ApplicationCore.Entities.UserAggregate;

namespace FirstResponder.ApplicationCore.Users.DTOs;

public class UserFormDTO
{
    [EmailAddress]
    public required string Email { get; set; }

    public required string FullName { get; set; }
    
    public required string PhoneNumber { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime? DateOfBirth { get; set; }

    public string? Address { get; set; }

    public string? PostalCode { get; set; }

    public string? City { get; set; }

    public RegionOfState Region { get; set; }

    public string? Notes { get; set; }

    public UserType UserType { get; set; }
    
    public User ToUser(User? targetUser = null)
    {
        if (targetUser == null)
        {
            return new User
            {
                Email = Email,
                FullName = FullName,
                PhoneNumber = PhoneNumber,
                DateOfBirth = (DateTime)DateOfBirth,
                Address = Address,
                PostalCode = PostalCode,
                City = City,
                Region = Region,
                Type = UserType,
                Notes = Notes
            };
        }

        targetUser.Email = Email;
        targetUser.FullName = FullName;
        targetUser.PhoneNumber = PhoneNumber;
        targetUser.DateOfBirth = (DateTime)DateOfBirth;
        targetUser.Address = Address;
        targetUser.PostalCode = PostalCode;
        targetUser.City = City;
        targetUser.Region = Region;
        targetUser.Type = UserType;
        targetUser.Notes = Notes;

        return targetUser;
    }
}