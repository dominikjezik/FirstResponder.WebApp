using MediatR;

namespace FirstResponder.ApplicationCore.Aeds.Commands;

public class CreateLanguageCommand : IRequest
{
    public string Name { get; private set; }

    public CreateLanguageCommand(string name)
    {
        Name = name;
    }
}