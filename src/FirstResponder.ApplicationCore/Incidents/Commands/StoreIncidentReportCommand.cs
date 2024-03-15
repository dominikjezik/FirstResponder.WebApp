using FirstResponder.ApplicationCore.Incidents.DTOs;
using MediatR;

namespace FirstResponder.ApplicationCore.Incidents.Commands;

public class StoreIncidentReportCommand : IRequest
{
    public Guid IncidentId { get; init; }
    
    public string ResponderId { get; init; }
    
    public IncidentReportFormDTO Report { get; init; }
}