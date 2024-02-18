using FirstResponder.ApplicationCore.Aeds.DTOs;
using FirstResponder.ApplicationCore.Aeds.Queries;
using FirstResponder.ApplicationCore.Common.Abstractions;
using MediatR;

namespace FirstResponder.ApplicationCore.Aeds.Handlers;

public class GetAllPublicAedItemsQueryHandler : IRequestHandler<GetAllPublicAedItemsQuery, IEnumerable<AedItemDTO>>
{
    private readonly IAedRepository _aedRepository;

    public GetAllPublicAedItemsQueryHandler(IAedRepository aedRepository)
    {
        _aedRepository = aedRepository;
    }
    
    public async Task<IEnumerable<AedItemDTO>> Handle(GetAllPublicAedItemsQuery request, CancellationToken cancellationToken)
    {
        return await _aedRepository.GetAllPublicAedItems();
    }
}