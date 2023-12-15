using FirstResponder.ApplicationCore.Abstractions;
using FirstResponder.ApplicationCore.Aeds.Commands;
using FirstResponder.ApplicationCore.Exceptions;
using MediatR;

namespace FirstResponder.ApplicationCore.Aeds.Handlers;

public class DeleteManufacturerCommandHandler : IRequestHandler<DeleteManufacturerCommand>
{
    private readonly IAedManufacturersRepository _aedManufacturersRepository;

    public DeleteManufacturerCommandHandler(IAedManufacturersRepository aedManufacturersRepository)
    {
        _aedManufacturersRepository = aedManufacturersRepository;
    }
    
    public async Task Handle(DeleteManufacturerCommand request, CancellationToken cancellationToken)
    {
        var manufacturer = await _aedManufacturersRepository.GetManufacturerById(request.ManufacturerId);
        
        if (manufacturer == null)
        {
            throw new EntityNotFoundException();
        }
        
        await _aedManufacturersRepository.DeleteManufacturer(manufacturer);
    }
}