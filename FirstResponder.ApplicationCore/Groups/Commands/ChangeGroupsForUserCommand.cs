using FirstResponder.ApplicationCore.Groups.DTOs;
using MediatR;

namespace FirstResponder.ApplicationCore.Groups.Commands;

public class ChangeGroupsForUserCommand : IRequest
{
    public ChangeGroupsForUserDTO ChangeGroupsForUserDTO { get; private set; }
    
    public ChangeGroupsForUserCommand(ChangeGroupsForUserDTO changeGroupsForUserDto)
    {
        ChangeGroupsForUserDTO = changeGroupsForUserDto;
    }
}