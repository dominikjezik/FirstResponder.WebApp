using FirstResponder.ApplicationCore.Aeds.Commands;
using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Common.Enums;
using FirstResponder.ApplicationCore.Common.Exceptions;
using FirstResponder.ApplicationCore.Common.Helpers;
using FirstResponder.ApplicationCore.Entities.AedAggregate;
using MediatR;

namespace FirstResponder.ApplicationCore.Aeds.Handlers;

public class UpdateAedCommandHandler : IRequestHandler<UpdateAedCommand, Aed?>
{
    private readonly IAedRepository _aedRepository;
    private readonly IAedModelsRepository _aedModelsRepository;
    private readonly IUsersRepository _usersRepository;
    private readonly IFileService _fileService;

    public UpdateAedCommandHandler(IAedRepository aedRepository, IAedModelsRepository aedModelsRepository, IUsersRepository usersRepository, IFileService fileService)
    {
        _aedRepository = aedRepository;
        _aedModelsRepository = aedModelsRepository;
        _usersRepository = usersRepository;
        _fileService = fileService;
    }
    
    public async Task<Aed?> Handle(UpdateAedCommand request, CancellationToken cancellationToken)
    {
        var aed = await _aedRepository.GetAedById(request.AedFormDto.AedId);

        if (aed == null)
        {
            throw new EntityNotFoundException();
        }

        request.AedFormDto.ToAed(aed);
        
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

        await _aedRepository.UpdateAed(aed);
        
        await HandlePhotosDelete(request, aed);
        
        await HandlePhotoUpload(request, aed);
        
        return aed;
    }

    private async Task HandlePhotosDelete(UpdateAedCommand request, Aed aed)
    {
        var photosIdsForDelete = request.PhotosToDelete;
        if (photosIdsForDelete == null || photosIdsForDelete.Length == 0)
        {
            return;
        }
        
        var photosForDelete = await _aedRepository.GetAedPhotosByIds(aed.Id, photosIdsForDelete);
        
        await _aedRepository.DeleteAedPhotosByIds(aed.Id, photosIdsForDelete);
        
        foreach (var photo in photosForDelete)
        {
            await _fileService.DeleteFile(photo.PhotoName);
        }
    }

    private async Task HandlePhotoUpload(UpdateAedCommand request, Aed aed)
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