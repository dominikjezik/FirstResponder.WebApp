using FirstResponder.ApplicationCore.Incidents.DTOs;
using MediatR;

namespace FirstResponder.ApplicationCore.Incidents.Commands;

public class CreateIncidentMessageCommand : IRequest<IncidentMessageDTO>
{
    public Guid IncidentId { get; private set; }
    
    public string UserId { get; private set; }
    
    public string MessageContent { get; private set; }
    
    public CreateIncidentMessageCommand(Guid incidentId, string userId, string messageContent)
    {
        IncidentId = incidentId;
        UserId = userId;
        MessageContent = messageContent;
    }
}