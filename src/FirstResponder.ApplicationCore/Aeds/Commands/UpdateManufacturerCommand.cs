using MediatR;

namespace FirstResponder.ApplicationCore.Aeds.Commands;

public class UpdateManufacturerCommand : IRequest
{
    public Guid ManufacturerId { get; private set; }
    
    public string Name { get; private set; }

    public UpdateManufacturerCommand(Guid manufacturerId, string name)
    {
        ManufacturerId = manufacturerId;
        Name = name;
    }
}