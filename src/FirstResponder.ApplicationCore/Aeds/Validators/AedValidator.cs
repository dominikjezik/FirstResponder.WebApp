using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Common.Enums;
using FirstResponder.ApplicationCore.Common.Exceptions;
using FirstResponder.ApplicationCore.Entities.AedAggregate;

namespace FirstResponder.ApplicationCore.Aeds.Validators;

public class AedValidator
{
    private readonly IAedModelsRepository _aedModelsRepository;
    private readonly IAedManufacturersRepository _aedManufacturersRepository;
    private readonly IAedLanguagesRepository _aedLanguagesRepository;
    
    public AedValidator(IAedModelsRepository aedModelsRepository, IAedManufacturersRepository aedManufacturersRepository, IAedLanguagesRepository aedLanguagesRepository)
    {
        _aedModelsRepository = aedModelsRepository;
        _aedManufacturersRepository = aedManufacturersRepository;
        _aedLanguagesRepository = aedLanguagesRepository;
    }
    
    public async Task ValidateRelatedEntities(Aed aed)
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

    public void ValidateAvailability(PublicAed.PublicAedAvailability availability)
    {
        if (availability.Type == AedAvailabilityType.OperatingHours)
        {
            var validationErrors = new Dictionary<string, string>();
            
            if (availability.TimeFrom == null)
            {
                validationErrors.Add("Availability.TimeFrom", "Zadajte čas od kedy je AED k dispozícii!");
            }
            
            if (availability.TimeTo == null)
            {
                validationErrors.Add("Availability.TimeTo", "Zadajte čas do kedy je AED k dispozícii!");
            }
            
            if (availability.TimeFrom > availability.TimeTo)
            {
                validationErrors.Add("Availability.TimeFrom", "Dostupnosť AED v čase 'od' nemôže byť neskôr ako v čase 'do'!");
            }
            
            if (validationErrors.Any())
            {
                throw new EntityValidationException(validationErrors);
            }
            
            availability.DateFrom = null;
            availability.DateTo = null;
        }
        else if (availability.Type == AedAvailabilityType.InTerm)
        {
            var validationErrors = new Dictionary<string, string>();
            
            if (availability.DateFrom == null)
            {
                validationErrors.Add("Availability.DateFrom", "Zadajte dátum od kedy je AED k dispozícii!");
            }
            
            if (availability.DateTo == null)
            {
                validationErrors.Add("Availability.DateTo", "Zadajte dátum do kedy je AED k dispozícii!");
            }
            
            if (availability.DateFrom > availability.DateTo)
            {
                validationErrors.Add("Availability.DateFrom", "Dostupnosť AED v termíne 'od' nemôže byť neskôr ako v termíne 'do'!");
            }
            
            if (validationErrors.Any())
            {
                throw new EntityValidationException(validationErrors);
            }
            
            availability.TimeFrom = null;
            availability.TimeTo = null;
            availability.Days = null;
        } 
        else
        {
            availability.TimeFrom = null;
            availability.TimeTo = null;
            availability.DateFrom = null;
            availability.DateTo = null;
            availability.Days = null;
        }
    }
}