using FirstResponder.ApplicationCore.Aeds.Commands;
using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Common.Exceptions;
using FirstResponder.ApplicationCore.Exceptions;
using MediatR;

namespace FirstResponder.ApplicationCore.Aeds.Handlers;

public class DeleteLanguageCommandHandler : IRequestHandler<DeleteLanguageCommand>
{
    private readonly IAedLanguagesRepository _aedLanguagesRepository;

    public DeleteLanguageCommandHandler(IAedLanguagesRepository aedLanguagesRepository)
    {
        _aedLanguagesRepository = aedLanguagesRepository;
    }
    
    public async Task Handle(DeleteLanguageCommand request, CancellationToken cancellationToken)
    {
        var language = await _aedLanguagesRepository.GetLanguageById(request.LanguageId);
        
        if (language == null)
        {
            throw new EntityNotFoundException();
        }
        
        await _aedLanguagesRepository.DeleteLanguage(language);
    }
}