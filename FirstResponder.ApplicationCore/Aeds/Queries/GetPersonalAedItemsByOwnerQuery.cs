using FirstResponder.ApplicationCore.Aeds.DTOs;
using MediatR;

namespace FirstResponder.ApplicationCore.Aeds.Queries;

public class GetPersonalAedItemsByOwnerQuery : IRequest<IEnumerable<AedItemDTO>>
{
    public Guid OwnerId { get; private set; }
    
    public GetPersonalAedItemsByOwnerQuery(Guid ownerId)
    {
        OwnerId = ownerId;
    }
}