using FirstResponder.ApplicationCore.Aeds.Commands;
using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Common.Exceptions;
using MediatR;

namespace FirstResponder.ApplicationCore.Aeds.Handlers;

public class UpdateLanguageCommandHandler : IRequestHandler<UpdateLanguageCommand>
{
    private readonly IAedLanguagesRepository _aedLanguagesRepository;

    public UpdateLanguageCommandHandler(IAedLanguagesRepository aedLanguagesRepository)
    {
        _aedLanguagesRepository = aedLanguagesRepository;
    }
    
    public async Task Handle(UpdateLanguageCommand request, CancellationToken cancellationToken)
    {
        var language = await _aedLanguagesRepository.GetLanguageById(request.LanguageId);
        
        if (language == null)
        {
            throw new EntityNotFoundException();
        }

        if (language.Name == request.Name)
        {
            return;
        }
        
        if (await _aedLanguagesRepository.LanguageExists(request.Name))
        {
            throw new EntityValidationException("Name", "Tento jazyk už existuje!");
        }
        
        language.Name = request.Name;
        
        await _aedLanguagesRepository.UpdateLanguage(language);
    }
}