using FirstResponder.ApplicationCore.Groups.DTOs;
using MediatR;

namespace FirstResponder.ApplicationCore.Groups.Commands;

public class ChangeUsersInGroupCommand : IRequest
{
    public ChangeUsersInGroupDTO ChangeUsersInGroupDto { get; set; }
    
    public ChangeUsersInGroupCommand(ChangeUsersInGroupDTO changeUsersInGroupDto)
    {
        ChangeUsersInGroupDto = changeUsersInGroupDto;
    }
}