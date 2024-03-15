using FirstResponder.ApplicationCore.Aeds.DTOs;
using FirstResponder.ApplicationCore.Aeds.Queries;
using FirstResponder.ApplicationCore.Common.Abstractions;
using MediatR;

namespace FirstResponder.ApplicationCore.Aeds.Handlers;

public class GetPersonalAedItemsByOwnerQueryHandler : IRequestHandler<GetPersonalAedItemsByOwnerQuery, IEnumerable<AedItemDTO>>
{
    private readonly IAedRepository _aedRepository;
    
    public GetPersonalAedItemsByOwnerQueryHandler(IAedRepository aedRepository)
    {
        _aedRepository = aedRepository;
    }
    
    public async Task<IEnumerable<AedItemDTO>> Handle(GetPersonalAedItemsByOwnerQuery request, CancellationToken cancellationToken)
    {
        return await _aedRepository.GetPersonalAedItemsByOwnerId(request.OwnerId);
    }
}