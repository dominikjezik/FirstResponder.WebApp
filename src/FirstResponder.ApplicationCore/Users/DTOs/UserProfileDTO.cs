using System.ComponentModel.DataAnnotations;
using FirstResponder.ApplicationCore.Common.Enums;
using FirstResponder.ApplicationCore.Entities.UserAggregate;

namespace FirstResponder.ApplicationCore.Users.DTOs;

public class UserProfileDTO
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string FullName { get; set; }
    
    [Required]
    public string PhoneNumber { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime? DateOfBirth { get; set; }

    public string? Address { get; set; }

    public string? PostalCode { get; set; }

    public string? City { get; set; }

    [EnumDataType(typeof(RegionOfState), ErrorMessage = "Neplatný región")]
    public RegionOfState Region { get; set; }

    public string? Notes { get; set; }
    
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
        targetUser.Notes = Notes;

        return targetUser;
    }
}