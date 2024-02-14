using FirstResponder.ApplicationCore.Entities.IncidentAggregate;
using MediatR;

namespace FirstResponder.ApplicationCore.Incidents.Queries;

public class GetIncidentByIdQuery : IRequest<Incident?>
{
    public string IncidentId { get; private set; }
    
    public GetIncidentByIdQuery(string incidentId)
    {
        IncidentId = incidentId;
    }
}