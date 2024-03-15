using FirstResponder.ApplicationCore.Entities.IncidentAggregate;
using FirstResponder.ApplicationCore.Incidents.DTOs;
using MediatR;

namespace FirstResponder.ApplicationCore.Incidents.Commands;

public class CreateIncidentCommand : IRequest<Incident>
{
    public IncidentFormDTO IncidentFormDTO { get; private set; }
    
    public CreateIncidentCommand(IncidentFormDTO incidentFormDto)
    {
        IncidentFormDTO = incidentFormDto;
    }
}