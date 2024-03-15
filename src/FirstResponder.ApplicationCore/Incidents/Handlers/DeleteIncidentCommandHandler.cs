using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Common.Exceptions;
using FirstResponder.ApplicationCore.Incidents.Commands;
using MediatR;

namespace FirstResponder.ApplicationCore.Incidents.Handlers;

public class DeleteIncidentCommandHandler : IRequestHandler<DeleteIncidentCommand>
{
    private readonly IIncidentsRepository _incidentsRepository;

    public DeleteIncidentCommandHandler(IIncidentsRepository incidentsRepository)
    {
        _incidentsRepository = incidentsRepository;
    }
    
    public async Task Handle(DeleteIncidentCommand request, CancellationToken cancellationToken)
    {
        var incident = await _incidentsRepository.GetIncidentById(request.IncidentId);
        
        if (incident == null)
        {
            throw new EntityNotFoundException();
        }
        
        await _incidentsRepository.DeleteIncident(incident);
    }
}