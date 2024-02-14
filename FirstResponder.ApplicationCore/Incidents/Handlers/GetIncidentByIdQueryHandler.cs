using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Entities.IncidentAggregate;
using FirstResponder.ApplicationCore.Incidents.Queries;
using MediatR;

namespace FirstResponder.ApplicationCore.Incidents.Handlers;

public class GetIncidentByIdQueryHandler : IRequestHandler<GetIncidentByIdQuery, Incident?>
{
    private readonly IIncidentsRepository _incidentsRepository;

    public GetIncidentByIdQueryHandler(IIncidentsRepository incidentsRepository)
    {
        _incidentsRepository = incidentsRepository;
    }
    
    public async Task<Incident?> Handle(GetIncidentByIdQuery request, CancellationToken cancellationToken)
    {
        if (Guid.TryParse(request.IncidentId, out Guid guid))
        {
            return await _incidentsRepository.GetIncidentById(guid);
        }

        return null;
    }
}