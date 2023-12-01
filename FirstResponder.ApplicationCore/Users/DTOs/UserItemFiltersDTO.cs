using FirstResponder.ApplicationCore.Enums;

namespace FirstResponder.ApplicationCore.Users.DTOs;

public class UserItemFiltersDTO
{
    public string? FullName { get; set; }
    
    public string? PhoneNumber { get; set; }
    
    public UserType? Type { get; set; }

    public RegionOfState? Region { get; set; }
}