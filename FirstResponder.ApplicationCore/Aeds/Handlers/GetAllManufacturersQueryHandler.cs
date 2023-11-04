using FirstResponder.ApplicationCore.Abstractions;
using FirstResponder.ApplicationCore.Aeds.Queries;
using FirstResponder.ApplicationCore.Entities.AedAggregate;
using MediatR;

namespace FirstResponder.ApplicationCore.Aeds.Handlers;

public class GetAllManufacturersQueryHandler : IRequestHandler<GetAllManufacturersQuery, IEnumerable<Manufacturer>>
{
    private readonly IAedRepository _aedRepository;

    public GetAllManufacturersQueryHandler(IAedRepository aedRepository)
    {
        _aedRepository = aedRepository;
    }
    
    public async Task<IEnumerable<Manufacturer>> Handle(GetAllManufacturersQuery request, CancellationToken cancellationToken)
    {
        return await _aedRepository.GetAllManufacturers();
    }
}