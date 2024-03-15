using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Incidents.Commands;
using MediatR;

namespace FirstResponder.ApplicationCore.Incidents.Handlers;

public class AssignResponderToIncidentsCommandHandler : IRequestHandler<AssignResponderToIncidentsCommand>
{
    private readonly IIncidentsRepository _incidentsRepository;

    public AssignResponderToIncidentsCommandHandler(IIncidentsRepository incidentsRepository)
    {
        _incidentsRepository = incidentsRepository;
    }

    public async Task Handle(AssignResponderToIncidentsCommand request, CancellationToken cancellationToken)
    {
        await _incidentsRepository.AssignResponderToIncidents(request.ResponderId, request.Incidents);
    }
}