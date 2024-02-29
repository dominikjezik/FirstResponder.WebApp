using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Incidents.Commands;
using MediatR;

namespace FirstResponder.ApplicationCore.Incidents.Handlers;

public class RequestDeviceLocationsCommandHandler : IRequestHandler<RequestDeviceLocationsCommand>
{
    private readonly IMessagingService _messagingService;

    public RequestDeviceLocationsCommandHandler(IMessagingService messagingService)
    {
        _messagingService = messagingService;
    }
    
    public async Task Handle(RequestDeviceLocationsCommand request, CancellationToken cancellationToken)
    {
        await _messagingService.RequestDeviceLocationsAsync();
    }
}