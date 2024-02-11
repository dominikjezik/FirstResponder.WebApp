using FirstResponder.ApplicationCore.Aeds.Commands;
using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Common.Enums;
using FirstResponder.ApplicationCore.Common.Helpers;
using FirstResponder.ApplicationCore.Entities.AedAggregate;
using FirstResponder.ApplicationCore.Exceptions;
using MediatR;

namespace FirstResponder.ApplicationCore.Aeds.Handlers;

public class CreateAedCommandHandler : IRequestHandler<CreateAedCommand, Aed>
{
    private readonly IAedRepository _aedRepository;
    private readonly IAedModelsRepository _aedModelsRepository;
    private readonly IUsersRepository _usersRepository;
    private readonly IFileService _fileService;

    public CreateAedCommandHandler(IAedRepository aedRepository, IAedModelsRepository aedModelsRepository, IUsersRepository usersRepository, IFileService fileService)
    {
        _aedRepository = aedRepository;
        _aedModelsRepository = aedModelsRepository;
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

        await ValidateRelatedEntities(aed);
        
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

    private async Task ValidateRelatedEntities(Aed aed)
    {
        if (aed.ModelId != null)
        {
            var model = await _aedModelsRepository.GetModelById(aed.ModelId.Value);
            
            if (model == null)
            {
                var errors = new Dictionary<string, string>();
                errors["ModelId"] = "Model AED neexistuje!";
                
                throw new EntityValidationException(errors);
            }
            
            if (model.ManufacturerId != aed.ManufacturerId)
            {
                var errors = new Dictionary<string, string>();
                errors["ModelId"] = "Model AED nepatri pod vyrobcu!";
                
                throw new EntityValidationException(errors);
            }
        }
    }
}