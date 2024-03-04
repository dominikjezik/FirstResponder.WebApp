using MediatR;

namespace FirstResponder.ApplicationCore.Notifications.Commands;

public class SendNotificationCommand : IRequest
{
    public Guid NotificationId { get; private set; }

    public SendNotificationCommand(Guid notificationId)
    {
        NotificationId = notificationId;
    }
}