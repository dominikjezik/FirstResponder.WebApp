using FirstResponder.ApplicationCore.Incidents.DTOs;
using MediatR;

namespace FirstResponder.ApplicationCore.Incidents.Commands;

public class AcceptIncidentCommand : IRequest<IncidentResponderItemDTO>
{
    public string ResponderId { get; init; }

    public Guid IncidentId { get; init; }
}