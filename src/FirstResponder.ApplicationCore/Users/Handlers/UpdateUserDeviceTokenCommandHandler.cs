using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Entities.UserAggregate;
using FirstResponder.ApplicationCore.Users.Commands;
using MediatR;

namespace FirstResponder.ApplicationCore.Users.Handlers;

public class UpdateUserDeviceTokenCommandHandler : IRequestHandler<UpdateUserDeviceTokenCommand>
{
    private readonly IDeviceTokensRepository _deviceTokensRepository;
    
    public UpdateUserDeviceTokenCommandHandler(IDeviceTokensRepository deviceTokensRepository)
    {
        _deviceTokensRepository = deviceTokensRepository;
    }
    
    public async Task Handle(UpdateUserDeviceTokenCommand request, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(request.UserId, out var userId))
        {
            return;
        }
        
        if (request.OldDeviceToken != null)
        {
            await _deviceTokensRepository.DeleteToken(request.OldDeviceToken, userId);
        }
        
        if (request.NewDeviceToken != null)
        {
            // Check if same token already exists
            var existingToken = await _deviceTokensRepository.DeviceTokenExists(request.NewDeviceToken, userId);
        
            if (existingToken)
            {
                return;
            }
            
            var deviceToken = new DeviceToken
            {
                Token = request.NewDeviceToken,
                UserId = userId
            };
            
            await _deviceTokensRepository.AddDeviceToken(deviceToken);
        }
    }
}