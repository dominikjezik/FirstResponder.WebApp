using FirstResponder.ApplicationCore.Courses.DTOs;
using MediatR;

namespace FirstResponder.ApplicationCore.Courses.Commands;

public class ChangeUsersInCourseCommand : IRequest
{
    public ChangeUsersInCourseDTO ChangeUsersInCourseDTO { get; private set; }
    
    public ChangeUsersInCourseCommand(ChangeUsersInCourseDTO changeUsersInCourseDTO)
    {
        ChangeUsersInCourseDTO = changeUsersInCourseDTO;
    }
}