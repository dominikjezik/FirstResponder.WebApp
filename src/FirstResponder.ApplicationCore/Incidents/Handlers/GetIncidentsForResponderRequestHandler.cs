using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Incidents.DTOs;
using FirstResponder.ApplicationCore.Incidents.Queries;
using MediatR;

namespace FirstResponder.ApplicationCore.Incidents.Handlers;

public class GetIncidentsForResponderRequestHandler : IRequestHandler<GetIncidentsForResponderRequest, IEnumerable<IncidentItemForResponderDTO>>
{
    private readonly IIncidentsRepository _incidentRepository;

    public GetIncidentsForResponderRequestHandler(IIncidentsRepository incidentRepository)
    {
        _incidentRepository = incidentRepository;
    }

    public async Task<IEnumerable<IncidentItemForResponderDTO>> Handle(GetIncidentsForResponderRequest request, CancellationToken cancellationToken)
    {
        return await _incidentRepository.GetUsersIncidents(request.ResponderId);
    }
}