using FirstResponder.ApplicationCore.Incidents.DTOs;
using MediatR;

namespace FirstResponder.ApplicationCore.Incidents.Queries;

public class GetIncidentMessagesQuery : IRequest<IEnumerable<IncidentMessageDTO>>
{
    public Guid IncidentId { get; private set; }
    
    public GetIncidentMessagesQuery(Guid incidentId)
    {
        IncidentId = incidentId;
    }
}