using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Common.Helpers;
using FirstResponder.ApplicationCore.Entities.UserAggregate;
using FirstResponder.ApplicationCore.Users.Commands;
using MediatR;

namespace FirstResponder.ApplicationCore.Users.Handlers;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, User>
{
    private readonly IUsersRepository _usersRepository;

    public CreateUserCommandHandler(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }
    
    public async Task<User> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        User? user = null;
        
        if (request.UserFormDTO != null)
        {
            user = request.UserFormDTO.ToUser();
            EntityValidator.Validate(user);
            await _usersRepository.AddUser(user);
        }
        else if (request.UserRegisterFormDTO != null)
        {
            user = request.UserRegisterFormDTO.ToUser();
            EntityValidator.Validate(user);
            await _usersRepository.AddUser(user, request.UserRegisterFormDTO.Password);
        }
        else
        {
            throw new ArgumentNullException();
        }

        return user;
    }
}