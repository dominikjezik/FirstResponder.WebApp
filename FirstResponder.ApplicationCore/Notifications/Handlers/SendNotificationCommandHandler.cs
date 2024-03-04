using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Common.Exceptions;
using FirstResponder.ApplicationCore.Entities.UserAggregate;
using FirstResponder.ApplicationCore.Notifications.Commands;
using MediatR;

namespace FirstResponder.ApplicationCore.Notifications.Handlers;

public class SendNotificationCommandHandler : IRequestHandler<SendNotificationCommand>
{
    private readonly IMessagingService _messagingService;
    private readonly INotificationsRepository _notificationsRepository;
    
    public SendNotificationCommandHandler(IMessagingService messagingService, INotificationsRepository notificationsRepository)
    {
        _messagingService = messagingService;
        _notificationsRepository = notificationsRepository;
    }
    
    public async Task Handle(SendNotificationCommand request, CancellationToken cancellationToken)
    {
        var notification = await _notificationsRepository.GetNotificationByIdWithUsers(request.NotificationId);
        
        if (notification == null)
        {
            throw new EntityNotFoundException("Notifikácia nebola nájdená");
        }
        
        await _messagingService.SendNotificationAsync(new List<User>(), "Máte novú notifikáciu", notification.Content);
    }
}