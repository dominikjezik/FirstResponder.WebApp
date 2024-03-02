using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Notifications.DTOs;
using FirstResponder.ApplicationCore.Notifications.Queries;
using MediatR;

namespace FirstResponder.ApplicationCore.Notifications.Handlers;

public class GetNotificationsQueryHandler : IRequestHandler<GetNotificationsQuery, IEnumerable<NotificationDTO>>
{
    private readonly INotificationsRepository _notificationsRepository;
    private const int PageSize = 30;
    
    public GetNotificationsQueryHandler(INotificationsRepository notificationsRepository)
    {
        _notificationsRepository = notificationsRepository;
    }
    
    public async Task<IEnumerable<NotificationDTO>> Handle(GetNotificationsQuery request, CancellationToken cancellationToken)
    {
        int pageNumber = request.PageNumber;

        if (pageNumber < 0)
        {
            pageNumber = 0;
        }
        
        var result = await _notificationsRepository.GetNotifications(pageNumber, PageSize, request.Filters);

        return result;
    }
}