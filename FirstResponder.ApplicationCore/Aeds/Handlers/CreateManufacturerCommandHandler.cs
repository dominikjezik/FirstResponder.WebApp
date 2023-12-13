using FirstResponder.ApplicationCore.Abstractions;
using FirstResponder.ApplicationCore.Aeds.Commands;
using FirstResponder.ApplicationCore.Entities.AedAggregate;
using FirstResponder.ApplicationCore.Exceptions;
using MediatR;

namespace FirstResponder.ApplicationCore.Aeds.Handlers;

public class CreateManufacturerCommandHandler : IRequestHandler<CreateManufacturerCommand>
{
    private readonly IAedRepository _aedRepository;

    public CreateManufacturerCommandHandler(IAedRepository aedRepository)
    {
        _aedRepository = aedRepository;
    }
    
    public async Task Handle(CreateManufacturerCommand request, CancellationToken cancellationToken)
    {
        var manufacturer = new Manufacturer
        {
            Name = request.Name
        };
        
        if (await _aedRepository.ManufacturerExists(manufacturer.Name))
        {
            var errors = new Dictionary<string, string>();
            errors["Name"] = "Výrobca s týmto názvom už existuje!";
            
            throw new EntityValidationException(errors);
        }
        
        await _aedRepository.AddManufacturer(manufacturer);
    }
}