using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Common.DTOs;
using FirstResponder.ApplicationCore.Entities.UserAggregate;
using FirstResponder.ApplicationCore.Notifications.DTOs;
using FirstResponder.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace FirstResponder.Infrastructure.Repositories;

public class NotificationsRepository : INotificationsRepository
{
    private readonly ApplicationDbContext _dbContext;
    
    public NotificationsRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Notification?> GetNotificationById(Guid notificationId)
    {
        return await _dbContext.Notifications
            .FirstOrDefaultAsync(n => n.Id == notificationId);
    }

    public async Task<Notification?> GetNotificationByIdWithUsers(Guid notificationId)
    {
        // TODO: nacitanie novej tabulky s device tokenmi
        
        return await _dbContext.Notifications
            .Include(n => n.Recipients)
            .FirstOrDefaultAsync(n => n.Id == notificationId);
    }

    public async Task<IEnumerable<NotificationDTO>> GetNotifications(int pageNumber, int pageSize, NotificationFiltersDTO? filtersDTO = null)
    {
        var query = _dbContext.Notifications
            .OrderByDescending(c => c.CreatedAt)
            .Join(
                _dbContext.Users,
                n => n.SenderId,
                u => u.Id,
                (n, u) => new { Notification = n, Sender = u }
            )
            .AsQueryable();

        if (filtersDTO != null)
        {
            query = query
                .Where(result =>
                    (filtersDTO.From == null || result.Notification.CreatedAt >= filtersDTO.From) &&
                    (filtersDTO.To == null || result.Notification.CreatedAt <= filtersDTO.To) &&
                    (string.IsNullOrEmpty(filtersDTO.Content) || result.Notification.Content.Contains(filtersDTO.Content))
                );
        }
        
        return await query
            .Skip(pageNumber * pageSize)
            .Take(pageSize)
            .Select(result => new NotificationDTO()
            {
                Id = result.Notification.Id,
                Content = result.Notification.Content,
                SenderName = result.Sender.FullName,
                CreatedAt = result.Notification.CreatedAt,
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<NotificationDTO>> GetNotificationsByUserIdAsync(Guid userId)
    {
        return await _dbContext.NotificationUser
            .Where(notificationUser => notificationUser.UserId == userId)
            .Include(notificationUser => notificationUser.Notification)
            .Join(
                _dbContext.Users,
                notificationUser => notificationUser.Notification.SenderId,
                user => user.Id,
                (notificationUser, user) => new { NotificationUser = notificationUser, Sender = user }
            )
            .Select(result => new NotificationDTO
            {
                Id = result.NotificationUser.Notification.Id,
                Content = result.NotificationUser.Notification.Content,
                SenderName = result.Sender.FullName,
                CreatedAt = result.NotificationUser.Notification.CreatedAt,
            })
            .ToListAsync();
    }

    public async Task AddNotification(Notification notification)
    {
        _dbContext.Notifications.Add(notification);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateNotification(Notification notification)
    {
        _dbContext.Notifications.Update(notification);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteNotification(Notification notification)
    {
        _dbContext.Notifications.Remove(notification);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<UserWithAssociationInfoDTO>> GetUsersWithNotificationInfoAsync(Guid notificationId, string searchQuery, int limitResultsCount, bool includeNotInNotification = false)
    {
        var queryable = _dbContext.Users
            .GroupJoin(
                _dbContext.NotificationUser.Where(notificationUser => notificationUser.NotificationId == notificationId),
                user => user.Id,
                notificationUser => notificationUser.UserId,
                (user, notificationUser) => new { User = user, NotificationUser = notificationUser }
            )
            .Where(notificationUser => 
                notificationUser.User.FullName.Contains(searchQuery) || 
                notificationUser.User.Email.Contains(searchQuery)
            )
            .Where(notificationUser => includeNotInNotification || notificationUser.NotificationUser.Any())
            .OrderByDescending(notificationUser => notificationUser.User.CreatedAt)
            .SelectMany(
                notificationUser => notificationUser.NotificationUser.DefaultIfEmpty(),
                (user, notificationUser) => new UserWithAssociationInfoDTO
                {
                    UserId = user.User.Id,
                    FullName = user.User.FullName,
                    Email = user.User.Email,
                    IsAssociated = notificationUser != null,
                }
            );

        if (limitResultsCount > 0)
        {
            queryable = queryable.Take(limitResultsCount);
        }
        
        return await queryable.ToListAsync();
    }

    public async Task ChangeUsersInNotification(Guid notificationId, IEnumerable<Guid> addUsers, IEnumerable<Guid> removeUsers)
    {
        // Users that are in the notification
        var usersNotification = await _dbContext.NotificationUser
            .Where(notificationUser => notificationUser.NotificationId == notificationId)
            .Select(notificationUser => notificationUser.UserId)
            .ToListAsync();
		
        // Filter out already added users
        addUsers = addUsers.Except(usersNotification);
		
        // Filter out users not in the notification
        removeUsers = removeUsers.Intersect(usersNotification);
		
        var addNotificationUsers = addUsers.Select(userId => new NotificationUser
        {
            NotificationId = notificationId,
            UserId = userId,
        });
		
        var removeNotificationUsers = removeUsers.Select(userId => new NotificationUser
        {
            NotificationId = notificationId,
            UserId = userId
        });
		
        _dbContext.NotificationUser.AddRange(addNotificationUsers);
        _dbContext.NotificationUser.RemoveRange(removeNotificationUsers);
		
        await _dbContext.SaveChangesAsync();
    }
}