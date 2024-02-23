using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Common.Enums;
using FirstResponder.ApplicationCore.Common.Exceptions;
using FirstResponder.ApplicationCore.Incidents.Commands;
using MediatR;

namespace FirstResponder.ApplicationCore.Incidents.Handlers;

public class DeclineIncidentCommandHandler : IRequestHandler<DeclineIncidentCommand>
{
    private readonly IIncidentsRepository _incidentsRepository;
    private readonly IUsersRepository _usersRepository;

    public DeclineIncidentCommandHandler(IIncidentsRepository incidentsRepository, IUsersRepository usersRepository)
    {
        _incidentsRepository = incidentsRepository;
        _usersRepository = usersRepository;
    }
    
    public async Task Handle(DeclineIncidentCommand request, CancellationToken cancellationToken)
    {
        var incident = await _incidentsRepository.GetIncidentById(request.IncidentId);
        
        if (incident == null)
        {
            throw new EntityNotFoundException();
        }
        
        if (incident.State == IncidentState.Completed || incident.State == IncidentState.Canceled)
        {
            throw new EntityValidationException("Zásah už bol ukončený alebo zrušený.");
        }
        
        if (!Guid.TryParse(request.ResponderId, out var responderId))
        {
            throw new EntityNotFoundException();
        }
        
        var user = await _usersRepository.GetUserById(responderId);
        
        if (user == null)
        {
            throw new EntityNotFoundException();
        }
        
        await _incidentsRepository.DeclineIncident(incident, user);
        
        // TODO: Menit stav z inprogress na created ak uz nie je ziadny responder?
        
        // TODO: Ak responder uz akceptoval ale teraz odmieta, upozorni zamestnanca na details incident page
    }
}