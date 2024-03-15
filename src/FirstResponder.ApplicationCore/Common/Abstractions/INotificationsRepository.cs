using FirstResponder.ApplicationCore.Common.DTOs;
using FirstResponder.ApplicationCore.Entities.UserAggregate;
using FirstResponder.ApplicationCore.Notifications.DTOs;

namespace FirstResponder.ApplicationCore.Common.Abstractions;

public interface INotificationsRepository
{
    Task<Notification?> GetNotificationById(Guid notificationId);
    
    Task<Notification?> GetNotificationByIdWithUsers(Guid notificationId);
    
    Task<IEnumerable<NotificationDTO>> GetNotifications(int pageNumber, int pageSize, NotificationFiltersDTO? filtersDTO = null);
    
    Task<IEnumerable<NotificationDTO>> GetNotificationsByUserIdAsync(Guid userId);

    Task AddNotification(Notification notification);
    
    Task UpdateNotification(Notification notification);
    
    Task DeleteNotification(Notification notification);
    
    Task<IEnumerable<UserWithAssociationInfoDTO>> GetUsersWithNotificationInfoAsync(Guid notificationId, string searchQuery, int limitResultsCount, bool includeNotInNotification = false);
	
    Task ChangeUsersInNotification(Guid notificationId, IEnumerable<Guid> addUsers, IEnumerable<Guid> removeUsers);
}