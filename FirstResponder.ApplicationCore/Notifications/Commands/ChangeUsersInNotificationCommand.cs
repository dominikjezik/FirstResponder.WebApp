using FirstResponder.ApplicationCore.Common.DTOs;
using MediatR;

namespace FirstResponder.ApplicationCore.Notifications.Commands;

public class ChangeUsersInNotificationCommand : IRequest
{
    public UsersAssociationChangeDTO UsersAssociationChangeDTO { get; private set; }
    
    public ChangeUsersInNotificationCommand(UsersAssociationChangeDTO usersAssociationChangeDTO)
    {
        UsersAssociationChangeDTO = usersAssociationChangeDTO;
    }
}