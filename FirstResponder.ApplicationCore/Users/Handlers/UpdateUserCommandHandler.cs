using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Common.Exceptions;
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
        var user = await _usersRepository.GetUserById(request.UserId);
        
        if (user == null)
        {
            throw new EntityNotFoundException();
        }
        
        request.UserFormDTO.ToUser(user);
        await _usersRepository.UpdateUser(user);
        
        return user;
    }
}