using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Common.Enums;
using FirstResponder.ApplicationCore.Common.Exceptions;
using FirstResponder.ApplicationCore.Entities.IncidentAggregate;
using FirstResponder.ApplicationCore.Incidents.Commands;
using MediatR;

namespace FirstResponder.ApplicationCore.Incidents.Handlers;

public class CloseIncidentCommandHandler : IRequestHandler<CloseIncidentCommand, Incident>
{
    private readonly IIncidentsRepository _incidentsRepository;

    public CloseIncidentCommandHandler(IIncidentsRepository incidentsRepository)
    {
        _incidentsRepository = incidentsRepository;
    }
    
    public async Task<Incident> Handle(CloseIncidentCommand request, CancellationToken cancellationToken)
    {
        var incident = await _incidentsRepository.GetIncidentById(request.IncidentId);
        
        if (incident == null)
        {
            throw new EntityNotFoundException();
        }
        
        if (incident.State == IncidentState.Canceled || incident.State == IncidentState.Completed)
        {
            return incident;
        }

        if (incident.State == IncidentState.Created)
        {
            incident.State = IncidentState.Canceled;
        } 
        else if (incident.State == IncidentState.InProgress)
        {
            incident.State = IncidentState.Completed;
        }
        
        incident.ResolvedAt = DateTime.Now;
        
        await _incidentsRepository.UpdateIncident(incident);
        
        return incident;
    }
}