using FirstResponder.ApplicationCore.Entities.UserAggregate;

namespace FirstResponder.ApplicationCore.Common.Abstractions;

public interface ITokenService
{
    string GenerateAccessToken(User user);
    
    Guid? GetUserIdFromExpiredToken(string token);

    Task<RefreshToken> GenerateAndStoreRefreshToken(User user);
    
    Task<RefreshToken?> GetRefreshTokenModel(string token, Guid userId);
    
    Task DeleteRefreshToken(RefreshToken refreshToken);
}