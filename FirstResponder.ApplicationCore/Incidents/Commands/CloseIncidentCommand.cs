using FirstResponder.ApplicationCore.Entities.IncidentAggregate;
using MediatR;

namespace FirstResponder.ApplicationCore.Incidents.Commands;

public class CloseIncidentCommand : IRequest<Incident>
{
    public Guid IncidentId { get; }

    public CloseIncidentCommand(Guid incidentId)
    {
        IncidentId = incidentId;
    }
}