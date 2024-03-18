using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Users.Commands;
using MediatR;

namespace FirstResponder.ApplicationCore.Users.Handlers;

public class LogoutUserCommandHandler : IRequestHandler<LogoutUserCommand>
{
    private readonly IRefreshTokensRepository _refreshTokensRepository;
    private readonly IDeviceTokensRepository _deviceTokensRepository;
    
    public LogoutUserCommandHandler(IRefreshTokensRepository refreshTokensRepository, IDeviceTokensRepository deviceTokensRepository)
    {
        _refreshTokensRepository = refreshTokensRepository;
        _deviceTokensRepository = deviceTokensRepository;
    }
    
    public async Task Handle(LogoutUserCommand request, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(request.UserId, out var userId))
        {
            return;
        }
        
        if (!string.IsNullOrEmpty(request.RefreshToken))
        {
            await _refreshTokensRepository.DeleteRefreshToken(request.RefreshToken, userId);
        }
        
        if (!string.IsNullOrEmpty(request.DeviceToken))
        {
            await _deviceTokensRepository.DeleteToken(request.DeviceToken, userId);
        }
    }
}