using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Common.Exceptions;
using FirstResponder.ApplicationCore.Incidents.DTOs;
using FirstResponder.ApplicationCore.Incidents.Queries;
using MediatR;

namespace FirstResponder.ApplicationCore.Incidents.Handlers;

public class GetIncidentForResponderRequestHandler : IRequestHandler<GetIncidentForResponderRequest, IncidentDTO?>
{
    private readonly IIncidentsRepository _incidentRepository;
    
    public GetIncidentForResponderRequestHandler(IIncidentsRepository incidentRepository)
    {
        _incidentRepository = incidentRepository;
    }
    
    public async Task<IncidentDTO?> Handle(GetIncidentForResponderRequest request, CancellationToken cancellationToken)
    {
        var incident = await _incidentRepository.GetIncidentWithRespondersById(request.IncidentId);

        if (incident == null)
        {
            return null;
        }
        
        var responder = incident.Responders.FirstOrDefault(r => r.ResponderId == request.ResponderId);
        
        if (responder == null)
        {
            throw new UnauthorizedException("Nepotvrdili ste účasť na tomto zásahu.");
        }
        
        return incident.ToDTO();
    }
}