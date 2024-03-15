using MediatR;

namespace FirstResponder.ApplicationCore.Aeds.Commands;

public class DeleteManufacturerCommand : IRequest
{
    public Guid ManufacturerId { get; private set; }
    
    public DeleteManufacturerCommand(Guid manufacturerId)
    {
        ManufacturerId = manufacturerId;
    }
}