using FirstResponder.ApplicationCore.Abstractions;
using FirstResponder.ApplicationCore.Aeds.Commands;
using FirstResponder.ApplicationCore.Entities.AedAggregate;
using FirstResponder.ApplicationCore.Enums;
using FirstResponder.ApplicationCore.Exceptions;
using FirstResponder.ApplicationCore.Helpers;
using MediatR;

namespace FirstResponder.ApplicationCore.Aeds.Handlers;

public class CreateAedCommandHandler : IRequestHandler<CreateAedCommand, Aed>
{
    private readonly IAedRepository _aedRepository;
    private readonly IUsersRepository _usersRepository;
    private readonly IFileService _fileService;

    public CreateAedCommandHandler(IAedRepository aedRepository, IUsersRepository usersRepository, IFileService fileService)
    {
        _aedRepository = aedRepository;
        _usersRepository = usersRepository;
        _fileService = fileService;
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
                var errors = new Dictionary<string, string>();
                errors["OwnerId"] = "Owner not found";
                
                throw new EntityValidationException(errors);
            }
        }
        
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