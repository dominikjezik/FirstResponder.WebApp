using FirstResponder.ApplicationCore.Entities;
using MediatR;

namespace FirstResponder.ApplicationCore.Users.Queries;

public class GetAllUsersQuery : IRequest<IEnumerable<User>>
{
}