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

    public CreateIncidentMessageCommandHandler(IIncidentsRepository incidentsRepository, IUsersRepository usersRepository)
    {
        _incidentsRepository = incidentsRepository;
        _usersRepository = usersRepository;
    }
    
    public async Task<IncidentMessageDTO> Handle(CreateIncidentMessageCommand request, CancellationToken cancellationToken)
    {
        var incident = await _incidentsRepository.GetIncidentById(request.IncidentId);
        
        if (incident == null)
        {
            throw new EntityNotFoundException("Incident nebol nájdený");
        }
        
        if (incident.State == IncidentState.Canceled || incident.State == IncidentState.Completed)
        {
            throw new EntityValidationException("Incident bol už uzavretý");
        }
        
        // parse request.UserId to Guid
        if (!Guid.TryParse(request.UserId, out var userId))
        {
            throw new EntityNotFoundException("Používateľ nebol nájdený");
        }
        
        var user = await _usersRepository.GetUserById(userId);
        
        if (user == null)
        {
            throw new EntityNotFoundException("Používateľ nebol nájdený");
        }
        
        return await _incidentsRepository.SendMessageToIncident(incident, user, request.MessageContent);
    }
}