using FirstResponder.ApplicationCore.Entities.IncidentAggregate;
using FirstResponder.ApplicationCore.Incidents.DTOs;
using MediatR;

namespace FirstResponder.ApplicationCore.Incidents.Commands;

public class UpdateIncidentCommand : IRequest<Incident>
{
    public Guid IncidentId { get; private set; }
    
    public IncidentFormDTO IncidentFormDto { get; private set; }
    
    public UpdateIncidentCommand(Guid incidentId, IncidentFormDTO incidentFormDto)
    {
        IncidentId = incidentId;
        IncidentFormDto = incidentFormDto;
    }
}