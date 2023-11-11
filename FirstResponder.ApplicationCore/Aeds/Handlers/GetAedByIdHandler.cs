using FirstResponder.ApplicationCore.Abstractions;
using FirstResponder.ApplicationCore.Aeds.Queries;
using FirstResponder.ApplicationCore.Entities.AedAggregate;
using MediatR;

namespace FirstResponder.ApplicationCore.Aeds.Handlers;

public class GetAedByIdHandler : IRequestHandler<GetAedByIdQuery, Aed?>
{
    private readonly IAedRepository _aedRepository;

    public GetAedByIdHandler(IAedRepository aedRepository)
    {
        _aedRepository = aedRepository;
    }

    public async Task<Aed?> Handle(GetAedByIdQuery request, CancellationToken cancellationToken)
    {
        if (Guid.TryParse(request.AedId, out Guid guid))
        {
            return await _aedRepository.GetAedById(guid);
        }

        return null;
    }
}