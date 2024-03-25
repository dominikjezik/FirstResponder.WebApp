using FirstResponder.ApplicationCore.Entities.UserAggregate;

namespace FirstResponder.ApplicationCore.Common.Abstractions;

public interface IMessagingService
{
    Task RequestDeviceLocationsAsync();
    
    Task<IList<DeviceToken>> SendNotificationAsync(IList<DeviceToken> deviceTokens, string title, string message);
    
    Task<IList<DeviceToken>> NotifyNewMessageInIncidentAsync(IList<DeviceToken> deviceTokens, string incidentId, string title, string message);
    
    Task<IList<DeviceToken>> NotifyIncidentUpdateAsync(IList<DeviceToken> deviceTokens, string incidentId, string title, string message);
}