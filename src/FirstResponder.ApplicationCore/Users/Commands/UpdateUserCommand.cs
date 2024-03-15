using FirstResponder.ApplicationCore.Entities.UserAggregate;
using FirstResponder.ApplicationCore.Users.DTOs;
using MediatR;

namespace FirstResponder.ApplicationCore.Users.Commands;

public class UpdateUserCommand : IRequest<User>
{
    public Guid UserId { get; private set; }
    
    public UserFormDTO? UserFormDTO { get; private set; }
    
    public UserProfileDTO? UserProfileDTO { get; private set; }

    public UpdateUserCommand(Guid userId, UserFormDTO userFormDTO)
    {
        UserId = userId;
        UserFormDTO = userFormDTO;
    }
    
    public UpdateUserCommand(Guid userId, UserProfileDTO userProfileDTO)
    {
        UserId = userId;
        UserProfileDTO = userProfileDTO;
    }
}