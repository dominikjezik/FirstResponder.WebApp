using FirstResponder.ApplicationCore.Aeds.DTOs;
using FirstResponder.ApplicationCore.Aeds.Queries;
using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Common.Enums;
using MediatR;

namespace FirstResponder.ApplicationCore.Aeds.Handlers;

public class GetAedItemsQueryHandler : IRequestHandler<GetAedItemsQuery, IEnumerable<AedItemDTO>>
{
    private readonly IAedRepository _aedRepository;
    private const int pageSize = 30;

    public GetAedItemsQueryHandler(IAedRepository aedRepository)
    {
        _aedRepository = aedRepository;
    }
    
    public async Task<IEnumerable<AedItemDTO>> Handle(GetAedItemsQuery request, CancellationToken cancellationToken)
    {
        int pageNumber = request.PageNumber;

        if (pageNumber < 0)
        {
            pageNumber = 0;
        }
        
        if (request.Filters.Type == AedGeneralType.Personal)
        {
            return await _aedRepository.GetPersonalAedItems(pageNumber, pageSize, request.Filters);
        }

        if (request.Filters.Type == AedGeneralType.Public)
        {
            return await _aedRepository.GetPublicAedItems(pageNumber, pageSize, request.Filters);
        }

        return await _aedRepository.GetAedItems(pageNumber, pageSize, request.Filters);
    }
}