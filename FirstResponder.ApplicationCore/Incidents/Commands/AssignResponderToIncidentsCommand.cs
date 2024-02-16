using FirstResponder.ApplicationCore.Entities.IncidentAggregate;
using MediatR;

namespace FirstResponder.ApplicationCore.Incidents.Commands;

public class AssignResponderToIncidentsCommand : IRequest
{
    public Guid ResponderId { get; init; }
    public IEnumerable<Incident> Incidents { get; init; }
}