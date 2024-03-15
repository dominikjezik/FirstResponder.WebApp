using FirstResponder.ApplicationCore.Aeds.DTOs;
using FirstResponder.ApplicationCore.Entities.AedAggregate;
using MediatR;

namespace FirstResponder.ApplicationCore.Aeds.Commands;

public class CreateAedCommand : IRequest<Aed>
{
    public AedFormDTO AedFormDto { get; private set; }
    
    public CreateAedCommand(AedFormDTO aedFormDto)
    {
        AedFormDto = aedFormDto;
    }
}