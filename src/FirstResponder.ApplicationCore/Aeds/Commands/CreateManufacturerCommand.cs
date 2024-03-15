using MediatR;

namespace FirstResponder.ApplicationCore.Aeds.Commands;

public class CreateManufacturerCommand : IRequest
{
    public string Name { get; private set; }

    public CreateManufacturerCommand(string name)
    {
        Name = name;
    }
}