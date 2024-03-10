using FirstResponder.ApplicationCore.Incidents.DTOs;
using MediatR;

namespace FirstResponder.ApplicationCore.Incidents.Queries;

public class GetIncidentForResponderRequest : IRequest<IncidentDTO?>
{
    public Guid IncidentId { get; private set; }
    
    public Guid ResponderId { get; private set; }
    
    public GetIncidentForResponderRequest(Guid incidentId, Guid responderId)
    {
        IncidentId = incidentId;
        ResponderId = responderId;
    }
}