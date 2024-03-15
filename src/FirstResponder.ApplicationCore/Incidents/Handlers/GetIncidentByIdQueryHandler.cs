using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Incidents.DTOs;
using FirstResponder.ApplicationCore.Incidents.Queries;
using MediatR;

namespace FirstResponder.ApplicationCore.Incidents.Handlers;

public class GetIncidentByIdQueryHandler : IRequestHandler<GetIncidentByIdQuery, IncidentDTO?>
{
    private readonly IIncidentsRepository _incidentsRepository;

    public GetIncidentByIdQueryHandler(IIncidentsRepository incidentsRepository)
    {
        _incidentsRepository = incidentsRepository;
    }
    
    public async Task<IncidentDTO?> Handle(GetIncidentByIdQuery request, CancellationToken cancellationToken)
    {
        if (Guid.TryParse(request.IncidentId, out Guid guid))
        {
            return await _incidentsRepository.GetIncidentDetailsById(guid);
        }

        return null;
    }
}