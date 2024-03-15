using FirstResponder.ApplicationCore.Incidents.DTOs;
using MediatR;

namespace FirstResponder.ApplicationCore.Incidents.Queries;

public class GetIncidentRespondersQuery : IRequest<IEnumerable<IncidentResponderItemDTO>>
{
    public Guid IncidentId { get; private set; }
    
    public GetIncidentRespondersQuery(Guid incidentId)
    {
        IncidentId = incidentId;
    }
}