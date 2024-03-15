using FirstResponder.ApplicationCore.Courses.DTOs;
using FirstResponder.ApplicationCore.Entities.CourseAggregate;
using MediatR;

namespace FirstResponder.ApplicationCore.Courses.Commands;

public class UpdateCourseCommand : IRequest<Course>
{
    public Guid CourseId { get; private set; }
    
    public CourseFormDTO CourseFormDTO { get; private set; }
    
    public UpdateCourseCommand(Guid courseId, CourseFormDTO courseFormDTO)
    {
        CourseId = courseId;
        CourseFormDTO = courseFormDTO;
    }
}