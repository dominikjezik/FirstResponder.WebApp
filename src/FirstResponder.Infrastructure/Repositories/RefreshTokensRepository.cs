using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Entities.UserAggregate;
using FirstResponder.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace FirstResponder.Infrastructure.Repositories;

public class RefreshTokensRepository : IRefreshTokensRepository
{
    private readonly ApplicationDbContext _dbContext;

    public RefreshTokensRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task AddRefreshToken(RefreshToken refreshToken)
    {
        _dbContext.RefreshTokens.Add(refreshToken);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<RefreshToken?> GetRefreshToken(string refreshToken)
    {
        return await _dbContext.RefreshTokens
            .Where(rt => rt.Token == refreshToken)
            .FirstOrDefaultAsync();
    }

    public async Task DeleteRefreshToken(RefreshToken refreshToken)
    {
        _dbContext.RefreshTokens.Remove(refreshToken);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteRefreshToken(string refreshToken, Guid userId)
    {
        var token = await _dbContext.RefreshTokens
            .Where(rt => rt.Token == refreshToken && rt.UserId == userId)
            .FirstOrDefaultAsync();
        
        if (token != null)
        {
            _dbContext.RefreshTokens.Remove(token);
            await _dbContext.SaveChangesAsync();
        }
    }
}