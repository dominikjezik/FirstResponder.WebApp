using FirstResponder.ApplicationCore.Abstractions;
using FirstResponder.ApplicationCore.Aeds.Queries;
using FirstResponder.ApplicationCore.Entities.AedAggregate;
using MediatR;

namespace FirstResponder.ApplicationCore.Aeds.Handlers;

public class GetAllLanguagesQueryHandler : IRequestHandler<GetAllLanguagesQuery, IEnumerable<Language>>
{
    private readonly IAedRepository _aedRepository;

    public GetAllLanguagesQueryHandler(IAedRepository aedRepository)
    {
        _aedRepository = aedRepository;
    }
    
    public async Task<IEnumerable<Language>> Handle(GetAllLanguagesQuery request, CancellationToken cancellationToken)
    {
        return await _aedRepository.GetAllLanguages();
    }
}