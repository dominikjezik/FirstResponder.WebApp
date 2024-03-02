using MediatR;

namespace FirstResponder.ApplicationCore.Notifications.Commands;

public class DeleteNotificationCommand : IRequest
{
    public Guid NotificationId { get; private set; }

    public DeleteNotificationCommand(Guid notificationId)
    {
        NotificationId = notificationId;
    }
}