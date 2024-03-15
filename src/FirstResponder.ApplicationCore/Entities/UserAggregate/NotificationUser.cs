namespace FirstResponder.ApplicationCore.Entities.UserAggregate;

public class NotificationUser
{
    public Guid NotificationId { get; set; }
    public Notification? Notification { get; set; }
	
    public Guid UserId { get; set; }
    public User? User { get; set; }
}