using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Common.Exceptions;
using FirstResponder.ApplicationCore.Entities.IncidentAggregate;
using FirstResponder.ApplicationCore.Incidents.Commands;
using MediatR;

namespace FirstResponder.ApplicationCore.Incidents.Handlers;

public class StoreIncidentReportCommandHandler : IRequestHandler<StoreIncidentReportCommand>
{
    private readonly IIncidentsRepository _incidentsRepository;
    
    public StoreIncidentReportCommandHandler(IIncidentsRepository incidentsRepository)
    {
        _incidentsRepository = incidentsRepository;
    }
    
    public async Task Handle(StoreIncidentReportCommand request, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(request.ResponderId, out var responderId))
        {
            throw new EntityNotFoundException();
        }
        
        var incident = await _incidentsRepository.GetIncidentResponder(request.IncidentId, responderId);
        
        if (incident == null)
        {
            throw new EntityNotFoundException();
        }
        
        if (incident.AcceptedAt == null)
        {
            throw new EntityValidationException("Responder has not accepted the incident");
        }

        var report = new IncidentReport
        {
            Details = request.Report.Details,
            AedUsed = request.Report.AedUsed,
            AedShocks = request.Report.AedShocks
        };
        
        await _incidentsRepository.CreateOrUpdateIncidentReport(incident, report);
    }
}