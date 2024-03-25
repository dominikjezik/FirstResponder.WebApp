using FirstResponder.ApplicationCore.Incidents.DTOs;
using MediatR;

namespace FirstResponder.ApplicationCore.Incidents.Queries;

public class GetIncidentsForResponderRequest : IRequest<IEnumerable<IncidentItemForResponderDTO>>
{
    public Guid ResponderId { get; private set; }
    
    public GetIncidentsForResponderRequest(Guid responderId)
    {
        ResponderId = responderId;
    }
}