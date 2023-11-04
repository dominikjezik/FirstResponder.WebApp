using FirstResponder.ApplicationCore.Aeds.DTOs;
using FirstResponder.ApplicationCore.Entities.AedAggregate;
using MediatR;

namespace FirstResponder.ApplicationCore.Aeds.Commands;

public class CreateAedCommand : IRequest<Aed>
{
    public CreateAedDTO CreateAedDto { get; private set; }
    
    public CreateAedCommand(CreateAedDTO createAedDto)
    {
        CreateAedDto = createAedDto;
    }
}