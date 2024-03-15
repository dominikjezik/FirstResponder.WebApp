using MediatR;

namespace FirstResponder.ApplicationCore.Aeds.Commands;

public class CreateModelCommand : IRequest
{
    public string Name { get; private set; }
    
    public Guid ManufacturerId { get; private set; }

    public CreateModelCommand(string name, Guid manufacturerId)
    {
        Name = name;
        ManufacturerId = manufacturerId;
    }
}