using MediatR;

namespace FirstResponder.ApplicationCore.Users.Commands;

public class LogoutUserCommand : IRequest
{
    public string? UserId { get; private set; }
    
    public string? RefreshToken { get; private set; }
    
    public string? DeviceToken { get; private set; }
    
    public LogoutUserCommand(string? userId, string? refreshToken, string? deviceToken)
    {
        UserId = userId;
        RefreshToken = refreshToken;
        DeviceToken = deviceToken;
    }
}