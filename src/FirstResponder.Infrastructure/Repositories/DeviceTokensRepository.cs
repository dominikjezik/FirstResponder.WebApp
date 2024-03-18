using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Entities.UserAggregate;
using FirstResponder.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace FirstResponder.Infrastructure.Repositories;

public class DeviceTokensRepository : IDeviceTokensRepository
{
    private readonly ApplicationDbContext _dbContext;
    
    public DeviceTokensRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task AddDeviceToken(DeviceToken deviceToken)
    {
        _dbContext.DeviceTokens.Add(deviceToken);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IList<DeviceToken>> GetDeviceTokensForUsers(IEnumerable<User> users)
    {
        var userIds = users.Select(u => u.Id);
        
        return await _dbContext.DeviceTokens
            .Where(dt => userIds.Contains(dt.UserId))
            .ToListAsync();
    }

    public async Task<bool> DeviceTokenExists(string deviceToken, Guid userId)
    {
        return await _dbContext.DeviceTokens
            .AnyAsync(dt => dt.UserId == userId && dt.Token == deviceToken);
    }

    public async Task DeleteTokens(IList<DeviceToken> deviceTokens)
    {
        _dbContext.DeviceTokens.RemoveRange(deviceTokens);
        await _dbContext.SaveChangesAsync();
    }
    
    public async Task DeleteToken(string deviceToken, Guid userId)
    {
        var token = await _dbContext.DeviceTokens
            .Where(dt => dt.UserId == userId && dt.Token == deviceToken)
            .FirstOrDefaultAsync();
        
        if (token != null)
        {
            _dbContext.DeviceTokens.Remove(token);
            await _dbContext.SaveChangesAsync();
        }
    }
}