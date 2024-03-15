using MediatR;

namespace FirstResponder.ApplicationCore.Aeds.Commands;

public class DeleteLanguageCommand : IRequest
{
    public Guid LanguageId { get; private set; }
    
    public DeleteLanguageCommand(Guid languageId)
    {
        LanguageId = languageId;
    }
}