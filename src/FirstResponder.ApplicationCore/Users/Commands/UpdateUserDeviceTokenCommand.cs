using MediatR;

namespace FirstResponder.ApplicationCore.Users.Commands;

public class UpdateUserDeviceTokenCommand : IRequest
{
    public string UserId { get; private set; }
    
    public string? NewDeviceToken { get; private set; }
    
    public string? OldDeviceToken { get; private set; }
    
    public UpdateUserDeviceTokenCommand(string userId, string? newDeviceToken = null, string? oldDeviceToken = null)
    {
        UserId = userId;
        NewDeviceToken = newDeviceToken;
        OldDeviceToken = oldDeviceToken;
    }
}