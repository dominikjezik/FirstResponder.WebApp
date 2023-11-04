using FirstResponder.ApplicationCore.Entities.AedAggregate;
using MediatR;

namespace FirstResponder.ApplicationCore.Aeds.Queries;

public class GetAllModelsQuery : IRequest<IEnumerable<Model>>
{
    
}
