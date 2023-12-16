using FirstResponder.ApplicationCore.Abstractions;
using FirstResponder.ApplicationCore.Aeds.Queries;
using FirstResponder.ApplicationCore.Entities.AedAggregate;
using MediatR;

namespace FirstResponder.ApplicationCore.Aeds.Handlers;

public class GetAllLanguagesQueryHandler : IRequestHandler<GetAllLanguagesQuery, IEnumerable<Language>>
{
    private readonly IAedLanguagesRepository _aedLanguagesRepository;

    public GetAllLanguagesQueryHandler(IAedLanguagesRepository aedLanguagesRepository)
    {
        _aedLanguagesRepository = aedLanguagesRepository;
    }
    
    public async Task<IEnumerable<Language>> Handle(GetAllLanguagesQuery request, CancellationToken cancellationToken)
    {
        return await _aedLanguagesRepository.GetAllLanguages();
    }
}