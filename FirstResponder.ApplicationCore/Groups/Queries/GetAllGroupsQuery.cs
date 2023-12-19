using FirstResponder.ApplicationCore.Entities.UserAggregate;
using MediatR;

namespace FirstResponder.ApplicationCore.Groups.Queries;

public class GetAllGroupsQuery : IRequest<IEnumerable<Group>>
{
}
