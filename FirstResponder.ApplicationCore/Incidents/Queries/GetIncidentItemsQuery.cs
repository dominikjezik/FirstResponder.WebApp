using FirstResponder.ApplicationCore.Incidents.DTOs;
using MediatR;

namespace FirstResponder.ApplicationCore.Incidents.Queries;

public class GetIncidentItemsQuery : IRequest<IEnumerable<IncidentItemDTO>>
{
    public int PageNumber { get; set; }
    
    public IncidentItemFiltersDTO Filters { get; set; }
}
