using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Notifications.Commands;
using MediatR;

namespace FirstResponder.ApplicationCore.Notifications.Handlers;

public class ChangeUsersInNotificationCommandHandler : IRequestHandler<ChangeUsersInNotificationCommand>
{
    private readonly INotificationsRepository _notificationsRepository;
    
    public ChangeUsersInNotificationCommandHandler(INotificationsRepository notificationsRepository)
    {
        _notificationsRepository = notificationsRepository;
    }
    
    public async Task Handle(ChangeUsersInNotificationCommand request, CancellationToken cancellationToken)
    {
        await _notificationsRepository.ChangeUsersInNotification(request.UsersAssociationChangeDTO.EntityId, request.UsersAssociationChangeDTO.CheckedOnUserIds, request.UsersAssociationChangeDTO.CheckedOffUserIds);
    }
}