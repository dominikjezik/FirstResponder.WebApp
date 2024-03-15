using FirstResponder.ApplicationCore.Common.DTOs;
using MediatR;

namespace FirstResponder.ApplicationCore.Courses.Commands;

public class ChangeUsersInCourseCommand : IRequest
{
    public UsersAssociationChangeDTO UsersAssociationChangeDTO { get; private set; }
    
    public ChangeUsersInCourseCommand(UsersAssociationChangeDTO usersAssociationChangeDTO)
    {
        UsersAssociationChangeDTO = usersAssociationChangeDTO;
    }
}