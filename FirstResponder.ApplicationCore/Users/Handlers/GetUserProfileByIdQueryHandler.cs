using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Users.DTOs;
using FirstResponder.ApplicationCore.Users.Queries;
using MediatR;

namespace FirstResponder.ApplicationCore.Users.Handlers;

public class GetUserProfileByIdQueryHandler : IRequestHandler<GetUserProfileByIdQuery, UserProfileDTO?>
{
    private readonly IUsersRepository _usersRepository;
    
    public GetUserProfileByIdQueryHandler(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }
    
    public async Task<UserProfileDTO?> Handle(GetUserProfileByIdQuery request, CancellationToken cancellationToken)
    {
        if (Guid.TryParse(request.UserId, out Guid guid))
        {
            return await _usersRepository.GetUserProfileById(guid);
        }

        return null;
    }
}