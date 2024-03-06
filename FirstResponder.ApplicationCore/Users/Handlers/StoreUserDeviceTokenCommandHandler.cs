using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Entities.UserAggregate;
using FirstResponder.ApplicationCore.Users.Commands;
using MediatR;

namespace FirstResponder.ApplicationCore.Users.Handlers;

public class StoreUserDeviceTokenCommandHandler : IRequestHandler<StoreUserDeviceTokenCommand>
{
    private readonly IDeviceTokensRepository _deviceTokensRepository;
    
    public StoreUserDeviceTokenCommandHandler(IDeviceTokensRepository deviceTokensRepository)
    {
        _deviceTokensRepository = deviceTokensRepository;
    }
    
    public async Task Handle(StoreUserDeviceTokenCommand request, CancellationToken cancellationToken)
    {
        var deviceTokenEntity = new DeviceToken
        {
            UserId = request.User.Id,
            Token = request.DeviceToken
        };
                
        await _deviceTokensRepository.AddDeviceToken(deviceTokenEntity);
    }
}