using FirstResponder.ApplicationCore.Abstractions;
using FirstResponder.ApplicationCore.Entities;
using FirstResponder.ApplicationCore.Entities.UserAggregate;
using FirstResponder.ApplicationCore.Users.Commands;
using MediatR;

namespace FirstResponder.ApplicationCore.Users.Handlers;

public class UpdateUserCommandHandler: IRequestHandler<UpdateUserCommand, User?>
{
    private readonly IUsersRepository _usersRepository;

    public UpdateUserCommandHandler(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }

    public async Task<User?> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = request.UserFormDTO.ToUser();
        await _usersRepository.UpdateUser(user);
        
        return user;
    }
}