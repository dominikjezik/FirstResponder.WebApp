using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Incidents.DTOs;
using FirstResponder.ApplicationCore.Incidents.Queries;
using MediatR;

namespace FirstResponder.ApplicationCore.Incidents.Handlers;

public class GetIncidentMessagesQueryHandler : IRequestHandler<GetIncidentMessagesQuery, IEnumerable<IncidentMessageDTO>>
{
    private readonly IIncidentsRepository _incidentsRepository;
    
    public GetIncidentMessagesQueryHandler(IIncidentsRepository incidentsRepository)
    {
        _incidentsRepository = incidentsRepository;
    }
    
    public async Task<IEnumerable<IncidentMessageDTO>> Handle(GetIncidentMessagesQuery request, CancellationToken cancellationToken)
    {
        return await _incidentsRepository.GetIncidentMessages(request.IncidentId);
    }
}