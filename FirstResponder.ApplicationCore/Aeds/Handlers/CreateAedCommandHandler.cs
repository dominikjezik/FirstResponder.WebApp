using FirstResponder.ApplicationCore.Aeds.Commands;
using FirstResponder.ApplicationCore.Aeds.Validators;
using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Common.Enums;
using FirstResponder.ApplicationCore.Common.Exceptions;
using FirstResponder.ApplicationCore.Common.Helpers;
using FirstResponder.ApplicationCore.Entities.AedAggregate;
using MediatR;

namespace FirstResponder.ApplicationCore.Aeds.Handlers;

public class CreateAedCommandHandler : IRequestHandler<CreateAedCommand, Aed>
{
    private readonly IAedRepository _aedRepository;
    private readonly IUsersRepository _usersRepository;
    private readonly IFileService _fileService;
    private readonly AedValidator _aedValidator;

    public CreateAedCommandHandler(IAedRepository aedRepository, IUsersRepository usersRepository, IFileService fileService, AedValidator aedValidator)
    {
        _aedRepository = aedRepository;
        _usersRepository = usersRepository;
        _fileService = fileService;
        _aedValidator = aedValidator;
    }
    
    public async Task<Aed> Handle(CreateAedCommand request, CancellationToken cancellationToken)
    {
        var aed = request.AedFormDto.ToAed();
        
        EntityValidator.Validate(aed);

        if (request.AedFormDto.GeneralType == AedGeneralType.Personal)
        {
            var exist = await _usersRepository.UserExists(request.AedFormDto.OwnerId);
            if (!exist)
            {
                throw new EntityValidationException("OwnerId", "Owner not found");
            }
        }
        else
        {
            var publicAed = aed as PublicAed;
            _aedValidator.ValidateAvailability(publicAed?.Availability);
        }

        await _aedValidator.ValidateRelatedEntities(aed);
        
        await _aedRepository.AddAed(aed);
        
        await HandlePhotosUpload(request, aed);
        
        return aed;
    }
    
    private async Task HandlePhotosUpload(CreateAedCommand request, Aed aed)
    {
        if (request.AedFormDto.AedPhotoFileUploadDTOs == null || request.AedFormDto.GeneralType != AedGeneralType.Public)
        {
            return;
        }
        
        foreach (var photo in request.AedFormDto.AedPhotoFileUploadDTOs)
        {
            var photoName = await _fileService.StoreFile(photo);
            
            var aedPhoto = new AedPhoto
            {
                PublicAedId = aed.Id,
                PhotoName = photoName
            };

            await _aedRepository.AddAedPhoto(aedPhoto);
        }
    }
}