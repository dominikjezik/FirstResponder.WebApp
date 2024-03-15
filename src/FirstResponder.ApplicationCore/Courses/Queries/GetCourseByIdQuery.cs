using FirstResponder.ApplicationCore.Courses.DTOs;
using MediatR;

namespace FirstResponder.ApplicationCore.Courses.Queries;

public class GetCourseByIdQuery : IRequest<CourseDTO?>
{
    public Guid CourseId { get; private set; }
    
    public GetCourseByIdQuery(Guid courseId)
    {
        CourseId = courseId;
    }
}