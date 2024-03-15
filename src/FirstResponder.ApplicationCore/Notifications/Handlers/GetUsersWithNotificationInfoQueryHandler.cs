using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Common.DTOs;
using FirstResponder.ApplicationCore.Notifications.Queries;
using MediatR;

namespace FirstResponder.ApplicationCore.Notifications.Handlers;

public class GetUsersWithNotificationInfoQueryHandler : IRequestHandler<GetUsersWithNotificationInfoQuery, IEnumerable<UserWithAssociationInfoDTO>>
{
    private readonly INotificationsRepository _notificationsRepository;
    
    public GetUsersWithNotificationInfoQueryHandler(INotificationsRepository notificationsRepository)
    {
        _notificationsRepository = notificationsRepository;
    }
    
    public async Task<IEnumerable<UserWithAssociationInfoDTO>> Handle(GetUsersWithNotificationInfoQuery request, CancellationToken cancellationToken)
    {
        bool includeNotInCourse = request.Query != "";
        int limitResultsCount = request.Query != "" ? 30 : 0;
		
        return await _notificationsRepository.GetUsersWithNotificationInfoAsync(request.NotificationId, request.Query, limitResultsCount, includeNotInCourse);
    }
}