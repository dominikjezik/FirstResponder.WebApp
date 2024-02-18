namespace FirstResponder.ApplicationCore.Users.DTOs;

public class UserItemDTO
{
    public Guid Id { get; set; }
    
    public string FullName { get; set; }
    
    public string Email { get; set; }
    
    public string PhoneNumber { get; set; }
    
    public string CreatedAt { get; set; }
    
    public string Type { get; set; }
    
    public string Region { get; set; }

    public string Address { get; set; }
}