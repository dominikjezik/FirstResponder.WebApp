using FirstResponder.ApplicationCore.Common.Abstractions;
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
}