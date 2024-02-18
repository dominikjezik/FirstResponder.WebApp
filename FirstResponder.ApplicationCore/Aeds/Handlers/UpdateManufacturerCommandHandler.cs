using FirstResponder.ApplicationCore.Aeds.Commands;
using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Common.Exceptions;
using MediatR;

namespace FirstResponder.ApplicationCore.Aeds.Handlers;

public class UpdateManufacturerCommandHandler : IRequestHandler<UpdateManufacturerCommand>
{
    private readonly IAedManufacturersRepository _aedManufacturersRepository;

    public UpdateManufacturerCommandHandler(IAedManufacturersRepository aedManufacturersRepository)
    {
        _aedManufacturersRepository = aedManufacturersRepository;
    }
    
    public async Task Handle(UpdateManufacturerCommand request, CancellationToken cancellationToken)
    {
        var manufacturer = await _aedManufacturersRepository.GetManufacturerById(request.ManufacturerId);
        
        if (manufacturer == null)
        {
            throw new EntityNotFoundException();
        }

        if (manufacturer.Name == request.Name)
        {
            return;
        }
        
        if (await _aedManufacturersRepository.ManufacturerExists(request.Name))
        {
            var errors = new Dictionary<string, string>();
            errors["Name"] = "Výrobca s týmto názvom už existuje!";
            
            throw new EntityValidationException(errors);
        }
        
        manufacturer.Name = request.Name;
        
        await _aedManufacturersRepository.UpdateManufacturer(manufacturer);
    }
}