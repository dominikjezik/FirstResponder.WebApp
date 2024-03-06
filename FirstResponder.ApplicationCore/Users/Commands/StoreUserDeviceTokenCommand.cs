using FirstResponder.ApplicationCore.Entities.UserAggregate;
using MediatR;

namespace FirstResponder.ApplicationCore.Users.Commands;

public class StoreUserDeviceTokenCommand : IRequest
{
    public string DeviceToken { get; private set; }
    
    public User User { get; private set; }
    
    public StoreUserDeviceTokenCommand(User user, string deviceToken)
    {
        User = user;
        DeviceToken = deviceToken;
    }
}