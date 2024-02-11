using FirstResponder.ApplicationCore.Aeds.Queries;
using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Entities.AedAggregate;
using MediatR;

namespace FirstResponder.ApplicationCore.Aeds.Handlers;

public class GetAllAedsQueryHandler : IRequestHandler<GetAllAedsQuery, IEnumerable<Aed>>
{
    private readonly IAedRepository _aedRepository;

    public GetAllAedsQueryHandler(IAedRepository aedRepository)
    {
        _aedRepository = aedRepository;
    }
    
    public async Task<IEnumerable<Aed>> Handle(GetAllAedsQuery request, CancellationToken cancellationToken)
    {
        return await _aedRepository.GetAllAedsWithOwners();
    }
}