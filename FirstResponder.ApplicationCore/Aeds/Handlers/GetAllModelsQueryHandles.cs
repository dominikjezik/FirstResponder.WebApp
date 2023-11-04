using FirstResponder.ApplicationCore.Abstractions;
using FirstResponder.ApplicationCore.Aeds.Queries;
using FirstResponder.ApplicationCore.Entities.AedAggregate;
using MediatR;

namespace FirstResponder.ApplicationCore.Aeds.Handlers;

public class GetAllModelsQueryHandles : IRequestHandler<GetAllModelsQuery, IEnumerable<Model>>
{
    private readonly IAedRepository _aedRepository;

    public GetAllModelsQueryHandles(IAedRepository aedRepository)
    {
        _aedRepository = aedRepository;
    }

    public async Task<IEnumerable<Model>> Handle(GetAllModelsQuery request, CancellationToken cancellationToken)
    {
        return await _aedRepository.GetAllModels();
    }
}