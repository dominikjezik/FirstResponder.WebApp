using FirstResponder.ApplicationCore.Abstractions;
using FirstResponder.ApplicationCore.Aeds.Commands;
using FirstResponder.ApplicationCore.Exceptions;
using MediatR;

namespace FirstResponder.ApplicationCore.Aeds.Handlers;

public class UpdateManufacturerCommandHandler : IRequestHandler<UpdateManufacturerCommand>
{
    private readonly IAedRepository _aedRepository;

    public UpdateManufacturerCommandHandler(IAedRepository aedRepository)
    {
        _aedRepository = aedRepository;
    }
    
    public async Task Handle(UpdateManufacturerCommand request, CancellationToken cancellationToken)
    {
        var manufacturer = await _aedRepository.GetManufacturerById(request.ManufacturerId);
        
        if (manufacturer == null)
        {
            throw new EntityNotFoundException();
        }

        if (manufacturer.Name == request.Name)
        {
            return;
        }
        
        if (await _aedRepository.ManufacturerExists(request.Name))
        {
            var errors = new Dictionary<string, string>();
            errors["Name"] = "Výrobca s týmto názvom už existuje!";
            
            throw new EntityValidationException(errors);
        }
        
        manufacturer.Name = request.Name;
        
        await _aedRepository.UpdateManufacturer(manufacturer);
    }
}