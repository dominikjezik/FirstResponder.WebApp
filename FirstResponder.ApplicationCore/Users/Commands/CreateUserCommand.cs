using FirstResponder.ApplicationCore.Entities.UserAggregate;
using FirstResponder.ApplicationCore.Users.DTOs;
using MediatR;

namespace FirstResponder.ApplicationCore.Users.Commands;

public class CreateUserCommand : IRequest<User>
{
    public UserFormDTO UserFormDTO { get; private set; }

    public string UserPassword { get; private set; }

    public CreateUserCommand(UserFormDTO userFormDTO, string userPassword)
    {
        UserFormDTO = userFormDTO;
        UserPassword = userPassword;
    }
}