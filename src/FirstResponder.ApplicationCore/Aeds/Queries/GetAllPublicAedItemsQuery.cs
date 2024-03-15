using FirstResponder.ApplicationCore.Aeds.DTOs;
using MediatR;

namespace FirstResponder.ApplicationCore.Aeds.Queries;

public class GetAllPublicAedItemsQuery : IRequest<IEnumerable<AedItemDTO>>
{
}