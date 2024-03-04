using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Notifications.DTOs;
using FirstResponder.ApplicationCore.Notifications.Queries;
using MediatR;

namespace FirstResponder.ApplicationCore.Notifications.Handlers;

public class GetUserNotificationsQueryHandler : IRequestHandler<GetUserNotificationsQuery, IEnumerable<NotificationDTO>>
{
    private readonly INotificationsRepository _notificationsRepository;
    
    public GetUserNotificationsQueryHandler(INotificationsRepository notificationsRepository)
    {
        _notificationsRepository = notificationsRepository;
    }
    
    public async Task<IEnumerable<NotificationDTO>> Handle(GetUserNotificationsQuery request, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(request.UserId, out var userId))
        {
            return new List<NotificationDTO>();
        }
        
        return await _notificationsRepository.GetNotificationsByUserIdAsync(userId);
    }
}