using FirstResponder.ApplicationCore.Common.Enums;
using FirstResponder.ApplicationCore.Incidents.DTOs;
using MediatR;

namespace FirstResponder.ApplicationCore.Incidents.Commands;

public class UpdateResponderLocationCommand : IRequest<IncidentResponderItemDTO?>
{
    public string ResponderId { get; init; }

    public Guid IncidentId { get; init; }
    
    public double Latitude { get; set; }
    
    public double Longitude { get; set; }
    
    public TypeOfResponderTransport? TypeOfTransport { get; set; }
}