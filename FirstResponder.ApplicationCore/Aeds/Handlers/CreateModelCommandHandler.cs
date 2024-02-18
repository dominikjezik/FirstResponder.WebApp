using FirstResponder.ApplicationCore.Aeds.Commands;
using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Common.Exceptions;
using FirstResponder.ApplicationCore.Entities.AedAggregate;
using MediatR;

namespace FirstResponder.ApplicationCore.Aeds.Handlers;

public class CreateModelCommandHandler : IRequestHandler<CreateModelCommand>
{
    private readonly IAedModelsRepository _aedModelsRepository;
    private readonly IAedManufacturersRepository _aedManufacturersRepository;

    public CreateModelCommandHandler(IAedModelsRepository aedModelsRepository, IAedManufacturersRepository aedManufacturersRepository)
    {
        _aedModelsRepository = aedModelsRepository;
        _aedManufacturersRepository = aedManufacturersRepository;
    }
    
    public async Task Handle(CreateModelCommand request, CancellationToken cancellationToken)
    {
        // Kontrola, ci vyrobca existuje
        var manufacturer = await _aedManufacturersRepository.GetManufacturerById(request.ManufacturerId);
        
        if (manufacturer == null)
        {
            throw new EntityValidationException("Zadaný výrobca neexistuje!");
        }
        
        var model = new Model
        {
            Name = request.Name,
            ManufacturerId = request.ManufacturerId
        };
        
        if (await _aedModelsRepository.ModelExists(model.Name, request.ManufacturerId))
        {
            var errors = new Dictionary<string, string>();
            errors["Name"] = "Model s týmto názvom už existuje!";
            
            throw new EntityValidationException(errors);
        }
        
        await _aedModelsRepository.AddModel(model);
    }
}