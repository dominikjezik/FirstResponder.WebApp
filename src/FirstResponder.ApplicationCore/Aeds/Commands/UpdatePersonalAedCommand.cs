using FirstResponder.ApplicationCore.Aeds.DTOs;
using MediatR;

namespace FirstResponder.ApplicationCore.Aeds.Commands;

public class UpdatePersonalAedCommand : IRequest
{
    public AedFormDTO AedForm { get; private set; }
    
    public Guid UserId { get; private set; }
    
    public UpdatePersonalAedCommand(AedFormDTO aedForm, Guid userId)
    {
        AedForm = aedForm;
        UserId = userId;
    }
}