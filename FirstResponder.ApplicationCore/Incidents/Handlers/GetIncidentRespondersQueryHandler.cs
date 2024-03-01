using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Incidents.DTOs;
using FirstResponder.ApplicationCore.Incidents.Queries;
using MediatR;

namespace FirstResponder.ApplicationCore.Incidents.Handlers;

public class GetIncidentRespondersQueryHandler : IRequestHandler<GetIncidentRespondersQuery, IEnumerable<IncidentResponderItemDTO>>
{
    private readonly IIncidentsRepository _incidentsRepository;
    
    public GetIncidentRespondersQueryHandler(IIncidentsRepository incidentsRepository)
    {
        _incidentsRepository = incidentsRepository;
    }
    
    public async Task<IEnumerable<IncidentResponderItemDTO>> Handle(GetIncidentRespondersQuery request, CancellationToken cancellationToken)
    {
        return await _incidentsRepository.GetIncidentResponders(request.IncidentId);
    }
}