using FirstResponder.ApplicationCore.Abstractions;
using FirstResponder.ApplicationCore.Aeds.Queries;
using FirstResponder.ApplicationCore.Entities.AedAggregate;
using MediatR;

namespace FirstResponder.ApplicationCore.Aeds.Handlers;

public class GetAllManufacturersQueryHandler : IRequestHandler<GetAllManufacturersQuery, IEnumerable<Manufacturer>>
{
    private readonly IAedManufacturersRepository _aedManufacturersRepository;

    public GetAllManufacturersQueryHandler(IAedManufacturersRepository aedManufacturersRepository)
    {
        _aedManufacturersRepository = aedManufacturersRepository;
    }
    
    public async Task<IEnumerable<Manufacturer>> Handle(GetAllManufacturersQuery request, CancellationToken cancellationToken)
    {
        return await _aedManufacturersRepository.GetAllManufacturers();
    }
}