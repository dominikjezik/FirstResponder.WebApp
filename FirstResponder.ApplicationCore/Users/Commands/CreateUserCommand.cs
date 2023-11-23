using FirstResponder.ApplicationCore.Entities;
using FirstResponder.ApplicationCore.Users.DTOs;
using MediatR;

namespace FirstResponder.ApplicationCore.Users.Commands;

public class CreateUserCommand : IRequest<User>
{
    public UserFormDTO UserFormDto { get; private set; }

    public string UserPassword { get; private set; }

    public CreateUserCommand(UserFormDTO userFormDto, string userPassword)
    {
        UserFormDto = userFormDto;
        UserPassword = userPassword;
    }
}