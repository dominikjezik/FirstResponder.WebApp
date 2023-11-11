using FirstResponder.ApplicationCore.Abstractions;
using FirstResponder.ApplicationCore.Aeds.Commands;
using FirstResponder.ApplicationCore.Entities.AedAggregate;
using FirstResponder.ApplicationCore.Exceptions;
using FirstResponder.ApplicationCore.Helpers;
using MediatR;

namespace FirstResponder.ApplicationCore.Aeds.Handlers;

public class UpdateAedCommandHandler : IRequestHandler<UpdateAedCommand, Aed?>
{
    private readonly IAedRepository _aedRepository;

    public UpdateAedCommandHandler(IAedRepository aedRepository)
    {
        _aedRepository = aedRepository;
    }
    
    public async Task<Aed?> Handle(UpdateAedCommand request, CancellationToken cancellationToken)
    {
        var aed = await _aedRepository.GetAedById(request.AedFormDto.AedId);

        if (aed == null)
        {
            throw new EntityNotFoundException();
        }

        request.AedFormDto.ToAed(aed);
        
        EntityValidator.Validate(aed);
        
        // TODO: prípadná biznis validácia

        await _aedRepository.UpdateAed(aed);
        
        return aed;
    }
}