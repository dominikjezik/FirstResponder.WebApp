using FirstResponder.ApplicationCore.Common.Enums;
using FirstResponder.ApplicationCore.Entities.UserAggregate;

namespace FirstResponder.ApplicationCore.Users.DTOs;

public class UserFormDTO
{
    public UserProfileDTO UserProfile { get; set; } = new();

    public UserType UserType { get; set; }
    
    public User ToUser(User? targetUser = null)
    {
        var user = UserProfile.ToUser(targetUser);
        user.Type = UserType;
        
        return user;
    }
}