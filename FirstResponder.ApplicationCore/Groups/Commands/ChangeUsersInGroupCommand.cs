using FirstResponder.ApplicationCore.Groups.DTOs;
using MediatR;

namespace FirstResponder.ApplicationCore.Groups.Commands;

public class ChangeUsersInGroupCommand : IRequest
{
    public ChangeUsersInGroupDTO ChangeUsersInGroupDTO { get; private set; }
    
    public ChangeUsersInGroupCommand(ChangeUsersInGroupDTO changeUsersInGroupDTO)
    {
        ChangeUsersInGroupDTO = changeUsersInGroupDTO;
    }
}