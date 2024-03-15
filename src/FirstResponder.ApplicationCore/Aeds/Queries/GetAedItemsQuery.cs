using FirstResponder.ApplicationCore.Aeds.DTOs;
using MediatR;

namespace FirstResponder.ApplicationCore.Aeds.Queries;

public class GetAedItemsQuery : IRequest<IEnumerable<AedItemDTO>>
{
    public int PageNumber { get; set; }
    
    public AedItemFiltersDTO Filters { get; set; }
}