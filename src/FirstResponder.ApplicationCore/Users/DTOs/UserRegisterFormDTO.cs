namespace FirstResponder.ApplicationCore.Users.DTOs;

public class UserRegisterFormDTO : UserProfileDTO
{
    public string Password { get; set; }
    
    public string? DeviceToken { get; set; }
}