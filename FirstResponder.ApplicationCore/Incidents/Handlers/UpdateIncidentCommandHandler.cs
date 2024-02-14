using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Common.Exceptions;
using FirstResponder.ApplicationCore.Entities.IncidentAggregate;
using FirstResponder.ApplicationCore.Incidents.Commands;
using MediatR;

namespace FirstResponder.ApplicationCore.Incidents.Handlers;

public class UpdateIncidentCommandHandler : IRequestHandler<UpdateIncidentCommand, Incident?>
{
    private readonly IIncidentsRepository _incidentsRepository;

    public UpdateIncidentCommandHandler(IIncidentsRepository incidentsRepository)
    {
        _incidentsRepository = incidentsRepository;
    }
    
    public async Task<Incident?> Handle(UpdateIncidentCommand request, CancellationToken cancellationToken)
    {
        var incident = await _incidentsRepository.GetIncidentById(request.IncidentId);
        
        if (incident == null)
        {
            throw new EntityNotFoundException();
        }
        
        request.IncidentFormDto.ToIncident(incident);
        
        await _incidentsRepository.UpdateIncident(incident);
        return incident;
    }
}