using FirstResponder.ApplicationCore.Abstractions;
using FirstResponder.ApplicationCore.Aeds.Commands;
using FirstResponder.ApplicationCore.Entities.AedAggregate;
using MediatR;

namespace FirstResponder.ApplicationCore.Aeds.Handlers;

public class CreateAedCommandHandler : IRequestHandler<CreateAedCommand, Aed>
{
    private readonly IAedRepository _aedRepository;

    public CreateAedCommandHandler(IAedRepository aedRepository)
    {
        _aedRepository = aedRepository;
    }
    
    public async Task<Aed> Handle(CreateAedCommand request, CancellationToken cancellationToken)
    {
        // TODO: biznis validácia
        // ak ide o stacionárny potom vytvoriť pod-entitu
        
        var aed = request.CreateAedDto.ToAed();
        await _aedRepository.AddAed(aed);

        return aed;
    }
}