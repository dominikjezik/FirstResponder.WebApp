using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Entities.UserAggregate;
using FirstResponder.ApplicationCore.Users.Queries;
using MediatR;

namespace FirstResponder.ApplicationCore.Users.Handlers;

public class GetUserEntityByIdQueryHandler : IRequestHandler<GetUserEntityByIdQuery, User?>
{
    private readonly IUsersRepository _userRepository;
    
    public GetUserEntityByIdQueryHandler(IUsersRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public Task<User?> Handle(GetUserEntityByIdQuery request, CancellationToken cancellationToken)
    {
        return _userRepository.GetUserById(request.UserId);
    }
}