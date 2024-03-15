using FirstResponder.ApplicationCore.Entities.UserAggregate;
using FirstResponder.ApplicationCore.Users.DTOs;
using MediatR;

namespace FirstResponder.ApplicationCore.Users.Commands;

public class CreateUserCommand : IRequest<User>
{
    public UserFormDTO? UserFormDTO { get; private set; }

    public UserRegisterFormDTO? UserRegisterFormDTO { get; private set; }
    
    public CreateUserCommand(UserFormDTO userFormDTO)
    {
        UserFormDTO = userFormDTO;
    }
    
    public CreateUserCommand(UserRegisterFormDTO userRegisterFormDTO)
    {
        UserRegisterFormDTO = userRegisterFormDTO;
    }
}