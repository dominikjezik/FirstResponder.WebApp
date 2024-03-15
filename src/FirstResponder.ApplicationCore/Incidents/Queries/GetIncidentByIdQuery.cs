using FirstResponder.ApplicationCore.Incidents.DTOs;
using MediatR;

namespace FirstResponder.ApplicationCore.Incidents.Queries;

public class GetIncidentByIdQuery : IRequest<IncidentDTO?>
{
    public string IncidentId { get; private set; }
    
    public GetIncidentByIdQuery(string incidentId)
    {
        IncidentId = incidentId;
    }
}