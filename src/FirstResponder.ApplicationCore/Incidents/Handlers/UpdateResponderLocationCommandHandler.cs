using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Common.Enums;
using FirstResponder.ApplicationCore.Common.Exceptions;
using FirstResponder.ApplicationCore.Incidents.Commands;
using FirstResponder.ApplicationCore.Incidents.DTOs;
using MediatR;

namespace FirstResponder.ApplicationCore.Incidents.Handlers;

public class UpdateResponderLocationCommandHandler : IRequestHandler<UpdateResponderLocationCommand, IncidentResponderItemDTO?>
{
    private readonly IIncidentsRepository _incidentsRepository;
    private readonly IUsersRepository _usersRepository;

    public UpdateResponderLocationCommandHandler(IIncidentsRepository incidentsRepository, IUsersRepository usersRepository)
    {
        _incidentsRepository = incidentsRepository;
        _usersRepository = usersRepository;
    }
    
    public async Task<IncidentResponderItemDTO?> Handle(UpdateResponderLocationCommand request, CancellationToken cancellationToken)
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
        
        var incidentResponder = await _incidentsRepository.UpdateResponderLocation(incident, responderId, request.Latitude, request.Longitude, request.TypeOfTransport);
        
        if (incidentResponder == null)
        {
            return null;
        }
        
        return new IncidentResponderItemDTO
        {
            ResponderId = incidentResponder.ResponderId,
            Latitude = incidentResponder.LastLatitude,
            Longitude = incidentResponder.LastLongitude,
            TypeOfTransport = incidentResponder.TypeOfTransport.ToString(),
        };
    }
}