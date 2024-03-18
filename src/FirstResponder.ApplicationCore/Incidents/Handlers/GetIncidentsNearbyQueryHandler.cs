using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Entities.IncidentAggregate;
using FirstResponder.ApplicationCore.Incidents.Queries;
using MediatR;

namespace FirstResponder.ApplicationCore.Incidents.Handlers;

public class GetIncidentsNearbyQueryHandler : IRequestHandler<GetIncidentsNearbyQuery, IEnumerable<Incident>>
{
    private readonly IIncidentsRepository _incidentsRepository;
    
    public GetIncidentsNearbyQueryHandler(IIncidentsRepository incidentsRepository)
    {
        _incidentsRepository = incidentsRepository;
    }
    
    public async Task<IEnumerable<Incident>> Handle(GetIncidentsNearbyQuery request, CancellationToken cancellationToken)
    {
        return await _incidentsRepository.GetOpenedIncidentsNearby(request.Latitude, request.Longitude, request.Radius, request.UserId);
    }
}