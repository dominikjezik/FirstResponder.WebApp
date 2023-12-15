using MediatR;

namespace FirstResponder.ApplicationCore.Aeds.Commands;

public class CreateModelCommand : IRequest
{
    public string Name { get; private set; }

    public CreateModelCommand(string name)
    {
        Name = name;
    }
}