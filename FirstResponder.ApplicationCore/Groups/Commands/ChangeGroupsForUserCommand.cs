using FirstResponder.ApplicationCore.Groups.DTOs;
using MediatR;

namespace FirstResponder.ApplicationCore.Groups.Commands;

public class ChangeGroupsForUserCommand : IRequest
{
    public ChangeGroupsForUserDTO ChangeGroupsForUserDto { get; set; }
    
    public ChangeGroupsForUserCommand(ChangeGroupsForUserDTO changeGroupsForUserDto)
    {
        ChangeGroupsForUserDto = changeGroupsForUserDto;
    }
}