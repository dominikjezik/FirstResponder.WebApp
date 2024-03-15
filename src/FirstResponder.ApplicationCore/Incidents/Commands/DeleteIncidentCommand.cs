using MediatR;

namespace FirstResponder.ApplicationCore.Incidents.Commands;

public class DeleteIncidentCommand : IRequest
{
    public Guid IncidentId { get; private set; }

    public DeleteIncidentCommand(Guid incidentId)
    {
        IncidentId = incidentId;
    }
}