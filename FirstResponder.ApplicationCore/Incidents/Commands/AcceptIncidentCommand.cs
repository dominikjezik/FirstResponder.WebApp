using MediatR;

namespace FirstResponder.ApplicationCore.Incidents.Commands;

public class AcceptIncidentCommand : IRequest
{
    public string ResponderId { get; init; }

    public Guid IncidentId { get; init; }
}