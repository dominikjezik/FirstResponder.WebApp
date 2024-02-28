using FirstResponder.ApplicationCore.Aeds.Commands;
using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Common.Exceptions;
using FirstResponder.ApplicationCore.Entities.AedAggregate;
using MediatR;

namespace FirstResponder.ApplicationCore.Aeds.Handlers;

public class CreateManufacturerCommandHandler : IRequestHandler<CreateManufacturerCommand>
{
    private readonly IAedManufacturersRepository _aedManufacturersRepository;

    public CreateManufacturerCommandHandler(IAedManufacturersRepository aedManufacturersRepository)
    {
        _aedManufacturersRepository = aedManufacturersRepository;
    }
    
    public async Task Handle(CreateManufacturerCommand request, CancellationToken cancellationToken)
    {
        var manufacturer = new Manufacturer
        {
            Name = request.Name
        };
        
        if (await _aedManufacturersRepository.ManufacturerExists(manufacturer.Name))
        {
            throw new EntityValidationException("Name", "Výrobca s týmto názvom už existuje!");
        }
        
        await _aedManufacturersRepository.AddManufacturer(manufacturer);
    }
}