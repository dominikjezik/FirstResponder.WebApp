using MediatR;

namespace FirstResponder.ApplicationCore.Aeds.Commands;

public class UpdateLanguageCommand : IRequest
{
    public Guid LanguageId { get; private set; }
    public string Name { get; private set; }

    public UpdateLanguageCommand(Guid languageId, string name)
    {
        LanguageId = languageId;
        Name = name;
    }
}