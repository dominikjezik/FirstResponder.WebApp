using FirstResponder.ApplicationCore.Entities.UserAggregate;

namespace FirstResponder.ApplicationCore.Common.Abstractions;

public interface IDeviceTokensRepository
{
    Task AddDeviceToken(DeviceToken deviceToken);
    
    Task<IList<DeviceToken>> GetDeviceTokensForUsers(IEnumerable<User> users);
    
    Task RemoveTokens(IList<DeviceToken> deviceTokens);
    
    Task RemoveDeviceToken(string deviceToken, Guid userId);
}