using FirstResponder.ApplicationCore.Abstractions;
using FirstResponder.ApplicationCore.Entities.UserAggregate;
using FirstResponder.ApplicationCore.Users.Queries;
using MediatR;

namespace FirstResponder.ApplicationCore.Users.Handlers;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, User?>
{
    private readonly IUsersRepository _usersRepository;

    public GetUserByIdQueryHandler(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }
    
    public async Task<User?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        if (Guid.TryParse(request.UserId, out Guid guid))
        {
            return await _usersRepository.GetUserWithDetailsById(guid);
        }

        return null;
    }
}