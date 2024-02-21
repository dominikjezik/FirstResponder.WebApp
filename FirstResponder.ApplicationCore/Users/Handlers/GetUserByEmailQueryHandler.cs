using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Entities.UserAggregate;
using FirstResponder.ApplicationCore.Users.Queries;
using MediatR;

namespace FirstResponder.ApplicationCore.Users.Handlers;

public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, User?>
{
    private readonly IUsersRepository _usersRepository;

    public GetUserByEmailQueryHandler(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }

    public async Task<User?> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
    {
        return await _usersRepository.GetUserByEmail(request.Email);
    }
}