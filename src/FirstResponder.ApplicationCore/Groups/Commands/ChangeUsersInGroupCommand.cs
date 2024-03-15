using FirstResponder.ApplicationCore.Common.DTOs;
using MediatR;

namespace FirstResponder.ApplicationCore.Groups.Commands;

public class ChangeUsersInGroupCommand : IRequest
{
    public UsersAssociationChangeDTO UsersAssociationChangeDTO { get; private set; }

    
    public ChangeUsersInGroupCommand(UsersAssociationChangeDTO usersAssociationChangeDTO)
    {
        UsersAssociationChangeDTO = usersAssociationChangeDTO;
    }
}