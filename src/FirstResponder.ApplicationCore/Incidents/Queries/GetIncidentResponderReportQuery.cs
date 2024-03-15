using FirstResponder.ApplicationCore.Incidents.DTOs;
using MediatR;

namespace FirstResponder.ApplicationCore.Incidents.Queries;

public class GetIncidentResponderReportQuery : IRequest<IncidentReportDTO?>
{
    public Guid IncidentId { get; private set; }
    
    public Guid ResponderId { get; private set; }
    
    public GetIncidentResponderReportQuery(Guid incidentId, Guid responderId)
    {
        IncidentId = incidentId;
        ResponderId = responderId;
    }
}