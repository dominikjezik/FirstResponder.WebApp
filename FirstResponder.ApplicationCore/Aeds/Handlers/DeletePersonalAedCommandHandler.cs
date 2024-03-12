using FirstResponder.ApplicationCore.Aeds.Commands;
using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Common.Exceptions;
using FirstResponder.ApplicationCore.Entities.AedAggregate;
using MediatR;

namespace FirstResponder.ApplicationCore.Aeds.Handlers;

public class DeletePersonalAedCommandHandler : IRequestHandler<DeletePersonalAedCommand>
{
    private readonly IAedRepository _aedRepository;
    
    public DeletePersonalAedCommandHandler(IAedRepository aedRepository)
    {
        _aedRepository = aedRepository;
    }
    
    public async Task Handle(DeletePersonalAedCommand request, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(request.AedId, out var aedGuid))
        {
            throw new EntityNotFoundException();
        }
        
        var aed = await _aedRepository.GetAedById(aedGuid);
        
        if (aed == null)
        {
            throw new EntityNotFoundException();
        }
        
        if (aed is not PersonalAed personalAed || personalAed.OwnerId != request.UserId)
        {
            throw new UnauthorizedException();
        }
        
        await _aedRepository.DeleteAed(aed);
    }
}