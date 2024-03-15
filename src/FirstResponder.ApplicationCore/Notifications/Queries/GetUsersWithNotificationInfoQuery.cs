using FirstResponder.ApplicationCore.Common.DTOs;
using MediatR;

namespace FirstResponder.ApplicationCore.Notifications.Queries;

public class GetUsersWithNotificationInfoQuery : IRequest<IEnumerable<UserWithAssociationInfoDTO>>
{
    public Guid NotificationId { get; set; }
    
    public string Query { get; set; }
    
    public GetUsersWithNotificationInfoQuery(Guid notificationId, string query)
    {
        Query = query;
        NotificationId = notificationId;
    }
}