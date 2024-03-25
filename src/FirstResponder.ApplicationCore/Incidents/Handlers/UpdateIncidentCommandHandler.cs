using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Common.Exceptions;
using FirstResponder.ApplicationCore.Entities.IncidentAggregate;
using FirstResponder.ApplicationCore.Incidents.Commands;
using MediatR;

namespace FirstResponder.ApplicationCore.Incidents.Handlers;

public class UpdateIncidentCommandHandler : IRequestHandler<UpdateIncidentCommand, Incident?>
{
    private readonly IIncidentsRepository _incidentsRepository;
    private readonly IDeviceTokensRepository _deviceTokensRepository;
    private readonly IMessagingService _messagingService;

    public UpdateIncidentCommandHandler(IIncidentsRepository incidentsRepository, IDeviceTokensRepository deviceTokensRepository, IMessagingService messagingService)
    {
        _incidentsRepository = incidentsRepository;
        _deviceTokensRepository = deviceTokensRepository;
        _messagingService = messagingService;
    }
    
    public async Task<Incident?> Handle(UpdateIncidentCommand request, CancellationToken cancellationToken)
    {
        var incident = await _incidentsRepository.GetIncidentWithRespondersById(request.IncidentId);
        
        if (incident == null)
        {
            throw new EntityNotFoundException();
        }
        
        request.IncidentFormDTO.ToIncident(incident);
        
        await _incidentsRepository.UpdateIncident(incident);
        
        // Notify responders about the incident update
        var deviceTokens = await _deviceTokensRepository.GetDeviceTokensForUsers(
            incident.Responders.Select(i => i.ResponderId));

        var failedTokens = await _messagingService.NotifyIncidentUpdateAsync(deviceTokens,
            request.IncidentId.ToString(), "Aktualizácia zásahu", "Informácie o zásahu boli aktualizované");
        
        await _deviceTokensRepository.DeleteTokens(failedTokens);
        
        return incident;
    }
}