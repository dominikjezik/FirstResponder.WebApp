namespace FirstResponder.ApplicationCore.Notifications.DTOs;

public class NotificationFiltersDTO
{
    public DateTime? From { get; set; }
    
    public DateTime? To { get; set; }
    
    public string? Content { get; set; }
}