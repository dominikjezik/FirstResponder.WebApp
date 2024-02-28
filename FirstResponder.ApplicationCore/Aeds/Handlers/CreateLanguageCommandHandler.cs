using FirstResponder.ApplicationCore.Aeds.Commands;
using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Common.Exceptions;
using FirstResponder.ApplicationCore.Entities.AedAggregate;
using MediatR;

namespace FirstResponder.ApplicationCore.Aeds.Handlers;

public class CreateLanguageCommandHandler : IRequestHandler<CreateLanguageCommand>
{
    private readonly IAedLanguagesRepository _aedLanguagesRepository;

    public CreateLanguageCommandHandler(IAedLanguagesRepository aedLanguagesRepository)
    {
        _aedLanguagesRepository = aedLanguagesRepository;
    }
    
    public async Task Handle(CreateLanguageCommand request, CancellationToken cancellationToken)
    {
        var language = new Language()
        {
            Name = request.Name
        };
        
        if (await _aedLanguagesRepository.LanguageExists(language.Name))
        {
            throw new EntityValidationException("Name", "Tento jazyk už existuje!");
        }
        
        await _aedLanguagesRepository.AddLanguage(language);
    }
}