using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Common.Enums;
using FirstResponder.ApplicationCore.Common.Exceptions;
using FirstResponder.ApplicationCore.Incidents.Commands;
using FirstResponder.ApplicationCore.Incidents.DTOs;
using MediatR;

namespace FirstResponder.ApplicationCore.Incidents.Handlers;

public class AcceptIncidentCommandHandler : IRequestHandler<AcceptIncidentCommand, IncidentResponderItemDTO>
{
    private readonly IIncidentsRepository _incidentsRepository;
    private readonly IUsersRepository _usersRepository;

    public AcceptIncidentCommandHandler(IIncidentsRepository incidentsRepository, IUsersRepository usersRepository)
    {
        _incidentsRepository = incidentsRepository;
        _usersRepository = usersRepository;
    }

    public async Task<IncidentResponderItemDTO> Handle(AcceptIncidentCommand request, CancellationToken cancellationToken)
    {
        var incident = await _incidentsRepository.GetIncidentById(request.IncidentId);
        
        if (incident == null)
        {
            throw new EntityNotFoundException();
        }
        
        if (incident.State == IncidentState.Completed || incident.State == IncidentState.Canceled)
        {
            throw new EntityValidationException("Zásah už bol ukončený alebo zrušený.");
        }
        
        if (!Guid.TryParse(request.ResponderId, out var responderId))
        {
            throw new EntityNotFoundException();
        }
        
        var user = await _usersRepository.GetUserById(responderId);
        
        if (user == null)
        {
            throw new EntityNotFoundException();
        }
        
        var incidentResponder = await _incidentsRepository.AcceptIncident(incident, user, request.Latitude, request.Longitude, request.TypeOfTransport);
        
        if (incident.State == IncidentState.Created)
        {
            incident.State = IncidentState.InProgress;
            await _incidentsRepository.UpdateIncident(incident);
        }

        return new IncidentResponderItemDTO
        {
            ResponderId = incidentResponder.ResponderId,
            FullName = user.FullName,
            AcceptedAt = incidentResponder.AcceptedAt,
            Latitude = incidentResponder.LastLatitude,
            Longitude = incidentResponder.LastLongitude,
            TypeOfTransport = incidentResponder.TypeOfTransport.ToString(),
        };
    }
}