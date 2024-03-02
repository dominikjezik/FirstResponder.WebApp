using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Common.Exceptions;
using FirstResponder.ApplicationCore.Notifications.Commands;
using MediatR;

namespace FirstResponder.ApplicationCore.Notifications.Handlers;

public class UpdateNotificationCommandHandler : IRequestHandler<UpdateNotificationCommand>
{
    private readonly INotificationsRepository _notificationRepository;
    
    public UpdateNotificationCommandHandler(INotificationsRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }
    
    public async Task Handle(UpdateNotificationCommand request, CancellationToken cancellationToken)
    {
        var notification = await _notificationRepository.GetNotificationById(request.NotificationId);
        
        if (notification == null)
        {
            throw new EntityNotFoundException();
        }
        
        notification.Content = request.Content;
        await _notificationRepository.UpdateNotification(notification);
    }
}