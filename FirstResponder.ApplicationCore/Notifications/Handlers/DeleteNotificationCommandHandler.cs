using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Common.Exceptions;
using FirstResponder.ApplicationCore.Notifications.Commands;
using MediatR;

namespace FirstResponder.ApplicationCore.Notifications.Handlers;

public class DeleteNotificationCommandHandler : IRequestHandler<DeleteNotificationCommand>
{
    private readonly INotificationsRepository _notificationsRepository;
    
    public DeleteNotificationCommandHandler(INotificationsRepository notificationsRepository)
    {
        _notificationsRepository = notificationsRepository;
    }
    
    public async Task Handle(DeleteNotificationCommand request, CancellationToken cancellationToken)
    {
        var notification = await _notificationsRepository.GetNotificationById(request.NotificationId);
        
        if (notification == null)
        {
            throw new EntityNotFoundException();
        }
        
        await _notificationsRepository.DeleteNotification(notification);
    }
}