using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Incidents.DTOs;
using FirstResponder.ApplicationCore.Incidents.Queries;
using MediatR;

namespace FirstResponder.ApplicationCore.Incidents.Handlers;

public class GetAedEventsQueryHandler : IRequestHandler<GetAedEventsQuery, IEnumerable<AedEventDTO>>
{
    private readonly IAedRepository _aedRepository;
    
    public GetAedEventsQueryHandler(IAedRepository aedRepository)
    {
        _aedRepository = aedRepository;
    }
    
    public async Task<IEnumerable<AedEventDTO>> Handle(GetAedEventsQuery request, CancellationToken cancellationToken)
    {
        return await _aedRepository.GetAedEvents(request.From, request.To);
    }
}