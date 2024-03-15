namespace FirstResponder.ApplicationCore.Notifications.DTOs;

public class NotificationDTO
{
    public Guid Id { get; set; }
    
    public string Content { get; set; }
    
    public string SenderName { get; set; }
    
    public DateTime CreatedAt { get; set; }
}