using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Users.DTOs;
using FirstResponder.ApplicationCore.Users.Queries;
using MediatR;

namespace FirstResponder.ApplicationCore.Users.Handlers;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDTO?>
{
    private readonly IUsersRepository _usersRepository;

    public GetUserByIdQueryHandler(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }
    
    public async Task<UserDTO?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        if (Guid.TryParse(request.UserId, out Guid guid))
        {
            return await _usersRepository.GetUserWithDetailsById(guid);
        }

        return null;
    }
}