using FirstResponder.ApplicationCore.Courses.DTOs;
using FirstResponder.ApplicationCore.Entities.CourseAggregate;
using MediatR;

namespace FirstResponder.ApplicationCore.Courses.Queries;

public class GetCoursesQuery : IRequest<IEnumerable<Course>>
{
    public int PageNumber { get; set; }
    
    public CourseFiltersDTO? Filters { get; set; }
}