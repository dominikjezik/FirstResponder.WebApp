using MediatR;

namespace FirstResponder.ApplicationCore.Notifications.Commands;

public class UpdateNotificationCommand : IRequest
{
    public Guid NotificationId { get; private set; }
    
    public string Content { get; private set; }
    
    public UpdateNotificationCommand(Guid notificationId, string content)
    {
        NotificationId = notificationId;
        Content = content;
    }
}