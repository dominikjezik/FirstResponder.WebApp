using FirstResponder.ApplicationCore.Common.DTOs;
using MediatR;

namespace FirstResponder.ApplicationCore.Courses.Queries;

public class GetUsersWithCourseInfoQuery : IRequest<IEnumerable<UserWithAssociationInfoDTO>>
{
    public Guid CourseId { get; set; }

    public string Query { get; set; }

    public GetUsersWithCourseInfoQuery(Guid courseId, string query)
    {
        Query = query;
        CourseId = courseId;
    }
}