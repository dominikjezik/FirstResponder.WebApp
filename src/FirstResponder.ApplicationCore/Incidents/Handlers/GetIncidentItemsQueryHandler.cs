using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Incidents.DTOs;
using FirstResponder.ApplicationCore.Incidents.Queries;
using MediatR;

namespace FirstResponder.ApplicationCore.Incidents.Handlers;

public class GetIncidentItemsQueryHandler : IRequestHandler<GetIncidentItemsQuery, IEnumerable<IncidentItemDTO>>
{
    private readonly IIncidentsRepository _incidentsRepository;

    public GetIncidentItemsQueryHandler(IIncidentsRepository incidentsRepository)
    {
        _incidentsRepository = incidentsRepository;
    }
    
    public async Task<IEnumerable<IncidentItemDTO>> Handle(GetIncidentItemsQuery request, CancellationToken cancellationToken)
    {
        int pageNumber = request.PageNumber;

        if (pageNumber < 0)
        {
            pageNumber = 0;
        }
        
        int pageSize = request.PageSize;
        
        if (pageSize < 0)
        {
            pageSize = 0;
        }

        return await _incidentsRepository.GetIncidentItems(pageNumber, pageSize, request.Filters);
    }
}