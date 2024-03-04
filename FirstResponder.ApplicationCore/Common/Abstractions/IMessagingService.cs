using FirstResponder.ApplicationCore.Entities.UserAggregate;

namespace FirstResponder.ApplicationCore.Common.Abstractions;

public interface IMessagingService
{
    Task RequestDeviceLocationsAsync();
    
    Task SendNotificationAsync(List<User> users, string title, string message);
    
    Task StoreDeviceTokenAsync(User user, string deviceToken);
}