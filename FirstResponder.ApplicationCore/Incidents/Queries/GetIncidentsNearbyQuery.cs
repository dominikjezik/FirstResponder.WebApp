using FirstResponder.ApplicationCore.Entities.IncidentAggregate;
using MediatR;

namespace FirstResponder.ApplicationCore.Incidents.Queries;

public class GetIncidentsNearbyQuery : IRequest<IEnumerable<Incident>>
{
    public double Latitude { get; init; }
    public double Longitude { get; init; }
    public double Radius { get; init; }
    public Guid? UserId { get; init; } = null;
}