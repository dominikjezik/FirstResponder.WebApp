using FirstResponder.ApplicationCore.Aeds.DTOs;
using FirstResponder.ApplicationCore.Entities.AedAggregate;
using MediatR;

namespace FirstResponder.ApplicationCore.Aeds.Commands;

public class CreatePersonalAedCommand : IRequest<PersonalAed>
{
    public AedFormDTO AedForm { get; private set; }
    
    public Guid OwnerId { get; private set; }
    
    public CreatePersonalAedCommand(AedFormDTO aedForm, Guid ownerId)
    {
        AedForm = aedForm;
        OwnerId = ownerId;
    }
}