using FirstResponder.ApplicationCore.Aeds.Commands;
using FirstResponder.ApplicationCore.Aeds.Validators;
using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Common.Enums;
using FirstResponder.ApplicationCore.Common.Exceptions;
using FirstResponder.ApplicationCore.Common.Helpers;
using FirstResponder.ApplicationCore.Entities.AedAggregate;
using MediatR;

namespace FirstResponder.ApplicationCore.Aeds.Handlers;

public class UpdatePersonalAedCommandHandler : IRequestHandler<UpdatePersonalAedCommand>
{
    private readonly IAedRepository _aedRepository;
    private readonly IUsersRepository _usersRepository;
    private readonly AedValidator _aedValidator;
    
    public UpdatePersonalAedCommandHandler(IAedRepository aedRepository, IUsersRepository usersRepository, AedValidator aedValidator)
    {
        _aedRepository = aedRepository;
        _usersRepository = usersRepository;
        _aedValidator = aedValidator;
    }
    
    public async Task Handle(UpdatePersonalAedCommand request, CancellationToken cancellationToken)
    {
        var aed = await _aedRepository.GetAedById(request.AedForm.AedId);
        
        if (aed is not PersonalAed personalAed || personalAed.OwnerId != request.UserId)
        {
            throw new UnauthorizedException();
        }
        
        if (aed == null)
        {
            throw new EntityNotFoundException();
        }
        
        request.AedForm.GeneralType = AedGeneralType.Personal;
        request.AedForm.OwnerId = request.UserId;
        
        request.AedForm.ToAed(aed);
        
        EntityValidator.Validate(aed);
        
        var exist = await _usersRepository.UserExists(request.AedForm.OwnerId);
        
        if (!exist)
        {
            throw new EntityValidationException("OwnerId", "Owner not found");
        }
        
        await _aedValidator.ValidateRelatedEntities(aed);
        
        await _aedRepository.UpdateAed(aed);
    }
}