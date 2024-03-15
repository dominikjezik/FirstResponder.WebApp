using FirstResponder.ApplicationCore.Entities.AedAggregate;
using MediatR;

namespace FirstResponder.ApplicationCore.Aeds.Queries;

public class GetAllManufacturersQuery : IRequest<IEnumerable<Manufacturer>>
{
}