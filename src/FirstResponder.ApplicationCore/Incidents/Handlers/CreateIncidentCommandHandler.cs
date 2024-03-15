using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Entities.IncidentAggregate;
using FirstResponder.ApplicationCore.Incidents.Commands;
using MediatR;

namespace FirstResponder.ApplicationCore.Incidents.Handlers;

public class CreateIncidentCommandHandler : IRequestHandler<CreateIncidentCommand, Incident>
{
    private readonly IIncidentsRepository _incidentsRepository;

    public CreateIncidentCommandHandler(IIncidentsRepository incidentsRepository)
    {
        _incidentsRepository = incidentsRepository;
    }
    
    public async Task<Incident> Handle(CreateIncidentCommand request, CancellationToken cancellationToken)
    {
        var incident = request.IncidentFormDTO.ToIncident();
        
        await _incidentsRepository.AddIncident(incident);
        
        return incident;
    }
}