using FirstResponder.ApplicationCore.Entities.CourseAggregate;
using MediatR;

namespace FirstResponder.ApplicationCore.Courses.Queries;

public class GetAllCourseTypesQuery : IRequest<IEnumerable<CourseType>>
{
}