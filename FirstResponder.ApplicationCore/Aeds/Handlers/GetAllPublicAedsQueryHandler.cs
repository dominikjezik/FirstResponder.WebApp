using FirstResponder.ApplicationCore.Abstractions;
using FirstResponder.ApplicationCore.Aeds.Queries;
using FirstResponder.ApplicationCore.Entities.AedAggregate;
using MediatR;

namespace FirstResponder.ApplicationCore.Aeds.Handlers;

public class GetAllPublicAedsQueryHandler : IRequestHandler<GetAllPublicAedsQuery, IEnumerable<PublicAed>>
{
    private readonly IAedRepository _aedRepository;

    public GetAllPublicAedsQueryHandler(IAedRepository aedRepository)
    {
        _aedRepository = aedRepository;
    }
    
    public async Task<IEnumerable<PublicAed>> Handle(GetAllPublicAedsQuery request, CancellationToken cancellationToken)
    {
        return await _aedRepository.GetAllPublicAeds();
    }
}