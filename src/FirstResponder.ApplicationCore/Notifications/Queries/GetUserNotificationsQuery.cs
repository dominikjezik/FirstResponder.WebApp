using FirstResponder.ApplicationCore.Notifications.DTOs;
using MediatR;

namespace FirstResponder.ApplicationCore.Notifications.Queries;

public class GetUserNotificationsQuery : IRequest<IEnumerable<NotificationDTO>>
{
    public string UserId { get; private set; }
    
    public int PageNumber { get; private set; }
    
    public GetUserNotificationsQuery(string userId, int pageNumber)
    {
        UserId = userId;
        PageNumber = pageNumber;
    }
}