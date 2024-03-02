using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Common.Exceptions;
using FirstResponder.ApplicationCore.Entities.UserAggregate;
using FirstResponder.ApplicationCore.Notifications.Commands;
using MediatR;

namespace FirstResponder.ApplicationCore.Notifications.Handlers;

public class CreateNotificationCommandHandler : IRequestHandler<CreateNotificationCommand>
{
    private readonly INotificationsRepository _notificationsRepository;
    
    public CreateNotificationCommandHandler(INotificationsRepository notificationsRepository)
    {
        _notificationsRepository = notificationsRepository;
    }
    
    public async Task Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(request.SenderId, out Guid senderId))
        {
            throw new EntityValidationException("Invalid sender id");
        }
        
        var notification = new Notification
        {
            Content = request.Content,
            SenderId = senderId
        };
        await _notificationsRepository.AddNotification(notification);
    }
}