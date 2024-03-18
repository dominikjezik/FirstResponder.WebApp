using FirstResponder.ApplicationCore.Entities.UserAggregate;

namespace FirstResponder.ApplicationCore.Common.Abstractions;

public interface IRefreshTokensRepository
{
    Task AddRefreshToken(RefreshToken refreshToken);
    
    Task<RefreshToken?> GetRefreshToken(string refreshToken);
    
    Task DeleteRefreshToken(RefreshToken refreshToken);
    
    Task DeleteRefreshToken(string refreshToken, Guid userId);
}