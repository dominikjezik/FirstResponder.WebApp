using FirstResponder.ApplicationCore.Entities.UserAggregate;
using FirstResponder.ApplicationCore.Notifications.DTOs;

namespace FirstResponder.ApplicationCore.Common.Abstractions;

public interface INotificationsRepository
{
    Task<Notification?> GetNotificationById(Guid notificationId);
    
    Task<IEnumerable<NotificationDTO>> GetNotifications(int pageNumber, int pageSize, NotificationFiltersDTO? filtersDTO = null);

    Task AddNotification(Notification notification);
    
    Task UpdateNotification(Notification notification);
    
    Task DeleteNotification(Notification notification);
}