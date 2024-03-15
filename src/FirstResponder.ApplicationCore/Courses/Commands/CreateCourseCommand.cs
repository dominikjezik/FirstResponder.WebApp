using FirstResponder.ApplicationCore.Courses.DTOs;
using FirstResponder.ApplicationCore.Entities.CourseAggregate;
using MediatR;

namespace FirstResponder.ApplicationCore.Courses.Commands;

public class CreateCourseCommand : IRequest<Course>
{
    public CourseFormDTO CourseFormDTO { get; private set; }
    
    public CreateCourseCommand(CourseFormDTO courseFormDto)
    {
        CourseFormDTO = courseFormDto;
    }
}