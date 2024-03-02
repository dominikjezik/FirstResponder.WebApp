using FirstResponder.ApplicationCore.Notifications.DTOs;
using MediatR;

namespace FirstResponder.ApplicationCore.Notifications.Queries;

public class GetNotificationsQuery : IRequest<IEnumerable<NotificationDTO>>
{
    public int PageNumber { get; set; }
    
    public NotificationFiltersDTO Filters { get; set; }
}