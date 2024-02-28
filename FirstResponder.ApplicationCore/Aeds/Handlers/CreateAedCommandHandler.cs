using FirstResponder.ApplicationCore.Aeds.Commands;
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
    private readonly IAedModelsRepository _aedModelsRepository;
    private readonly IAedManufacturersRepository _aedManufacturersRepository;
    private readonly IAedLanguagesRepository _aedLanguagesRepository;
    private readonly IUsersRepository _usersRepository;
    private readonly IFileService _fileService;

    public CreateAedCommandHandler(IAedRepository aedRepository, IAedModelsRepository aedModelsRepository, IAedManufacturersRepository aedManufacturersRepository, IAedLanguagesRepository aedLanguagesRepository, IUsersRepository usersRepository, IFileService fileService)
    {
        _aedRepository = aedRepository;
        _aedModelsRepository = aedModelsRepository;
        _aedManufacturersRepository = aedManufacturersRepository;
        _aedLanguagesRepository = aedLanguagesRepository;
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
                throw new EntityValidationException("OwnerId", "Owner not found");
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
        if (aed.ManufacturerId != null)
        {
            var manufacturer = await _aedManufacturersRepository.GetManufacturerById(aed.ManufacturerId.Value);
            
            if (manufacturer == null)
            {
                throw new EntityValidationException("ManufacturerId", "Vyrobca AED neexistuje!");
            }
        }
        
        if (aed.ModelId != null)
        {
            var model = await _aedModelsRepository.GetModelById(aed.ModelId.Value);
            
            if (model == null)
            {
                throw new EntityValidationException("ModelId", "Model AED neexistuje!");
            }
            
            if (model.ManufacturerId != aed.ManufacturerId)
            {
                throw new EntityValidationException("ModelId", "Model AED nepatri pod vyrobcu!");
            }
        }
        
        if (aed.LanguageId != null)
        {
            var language = await _aedLanguagesRepository.GetLanguageById(aed.LanguageId.Value);
            
            if (language == null)
            {
                throw new EntityValidationException("LanguageId", "Jazyk AED neexistuje!");
            }
        }
    }
}