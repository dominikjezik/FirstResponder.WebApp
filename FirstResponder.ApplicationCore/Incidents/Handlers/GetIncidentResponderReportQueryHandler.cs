using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Incidents.DTOs;
using FirstResponder.ApplicationCore.Incidents.Queries;
using MediatR;

namespace FirstResponder.ApplicationCore.Incidents.Handlers;

public class GetIncidentResponderReportQueryHandler : IRequestHandler<GetIncidentResponderReportQuery, IncidentReportDTO?>
{
    private readonly IIncidentsRepository _incidentsRepository;
    
    public GetIncidentResponderReportQueryHandler(IIncidentsRepository incidentsRepository)
    {
        _incidentsRepository = incidentsRepository;
    }
    
    public async Task<IncidentReportDTO?> Handle(GetIncidentResponderReportQuery request, CancellationToken cancellationToken)
    {
        return await _incidentsRepository.GetIncidentReport(request.IncidentId, request.ResponderId);
    }
}