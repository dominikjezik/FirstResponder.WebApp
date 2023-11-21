using FirstResponder.ApplicationCore.Entities;
using FirstResponder.ApplicationCore.Users.DTOs;
using MediatR;

namespace FirstResponder.ApplicationCore.Users.Commands;

public class UpdateUserCommand : IRequest<User>
{
    public UserFormDTO UserFormDTO { get; private set; }

    public UpdateUserCommand(UserFormDTO userFormDTO)
    {
        UserFormDTO = userFormDTO;
    }
}