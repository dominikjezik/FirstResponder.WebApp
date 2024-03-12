using FirstResponder.ApplicationCore.Aeds.Commands;
using FirstResponder.ApplicationCore.Aeds.Validators;
using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Common.Enums;
using FirstResponder.ApplicationCore.Common.Exceptions;
using FirstResponder.ApplicationCore.Common.Helpers;
using FirstResponder.ApplicationCore.Entities.AedAggregate;
using MediatR;

namespace FirstResponder.ApplicationCore.Aeds.Handlers;

public class CreatePersonalAedCommandHandler : IRequestHandler<CreatePersonalAedCommand, PersonalAed>
{
    private readonly IAedRepository _aedRepository;
    private readonly IUsersRepository _usersRepository;
    private readonly AedValidator _aedValidator;
    
    public CreatePersonalAedCommandHandler(IAedRepository aedRepository, IUsersRepository usersRepository, AedValidator aedValidator)
    {
        _aedRepository = aedRepository;
        _usersRepository = usersRepository;
        _aedValidator = aedValidator;
    }
    
    public async Task<PersonalAed> Handle(CreatePersonalAedCommand request, CancellationToken cancellationToken)
    {
        request.AedForm.GeneralType = AedGeneralType.Personal;
        request.AedForm.OwnerId = request.OwnerId;
        
        var aed = request.AedForm.ToAed();
        
        EntityValidator.Validate(aed);
        
        var exist = await _usersRepository.UserExists(request.OwnerId);
        if (!exist)
        {
            throw new EntityValidationException("OwnerId", "Owner not found");
        }
        
        await _aedValidator.ValidateRelatedEntities(aed);
        
        await _aedRepository.AddAed(aed);
        
        return (aed as PersonalAed)!;
    }
}