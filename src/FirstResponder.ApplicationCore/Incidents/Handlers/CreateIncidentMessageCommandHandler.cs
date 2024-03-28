using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Common.Enums;
using FirstResponder.ApplicationCore.Common.Exceptions;
using FirstResponder.ApplicationCore.Incidents.Commands;
using FirstResponder.ApplicationCore.Incidents.DTOs;
using MediatR;

namespace FirstResponder.ApplicationCore.Incidents.Handlers;

public class CreateIncidentMessageCommandHandler : IRequestHandler<CreateIncidentMessageCommand, IncidentMessageDTO>
{
    private readonly IIncidentsRepository _incidentsRepository;
    private readonly IUsersRepository _usersRepository;
    private readonly IDeviceTokensRepository _deviceTokensRepository;
    private readonly IMessagingService _messagingService;

    public CreateIncidentMessageCommandHandler(IIncidentsRepository incidentsRepository, IUsersRepository usersRepository, IDeviceTokensRepository deviceTokensRepository, IMessagingService messagingService)
    {
        _incidentsRepository = incidentsRepository;
        _usersRepository = usersRepository;
        _deviceTokensRepository = deviceTokensRepository;
        _messagingService = messagingService;
    }
    
    public async Task<IncidentMessageDTO> Handle(CreateIncidentMessageCommand request, CancellationToken cancellationToken)
    {
        var incident = await _incidentsRepository.GetIncidentWithRespondersById(request.IncidentId);
        
        if (incident == null)
        {
            throw new EntityNotFoundException("Incident nebol nájdený");
        }
        
        if (incident.State == IncidentState.Canceled || incident.State == IncidentState.Completed)
        {
            throw new EntityValidationException("Incident bol už uzavretý");
        }
        
        if (!Guid.TryParse(request.UserId, out var userId))
        {
            throw new EntityNotFoundException("Používateľ nebol nájdený");
        }
        
        var user = await _usersRepository.GetUserById(userId);
        
        if (user == null)
        {
            throw new EntityNotFoundException("Používateľ nebol nájdený");
        }
        
        // Check if user is in responders of the incident
        if (request.IsMessageFromResponder)
        {
            var incidentResponder = await _incidentsRepository.GetIncidentResponder(request.IncidentId, userId);
        
            if (incidentResponder?.AcceptedAt == null)
            {
                throw new EntityValidationException("Responder has not accepted the incident");
            }
        }
        
        var message = await _incidentsRepository.SendMessageToIncident(incident, user, request.MessageContent);

        // Notify responders about new message in incident
        var deviceTokens = await _deviceTokensRepository.GetDeviceTokensForUsers(
            incident.Responders.Select(i => i.ResponderId));
        
        var failedTokens = await _messagingService.NotifyNewMessageInIncidentAsync(deviceTokens, 
            request.IncidentId, "Nová správa v zásahu", message.Content, user.FullName, user.Id);
        
        await _deviceTokensRepository.DeleteTokens(failedTokens);
        
        return message;
    }
}