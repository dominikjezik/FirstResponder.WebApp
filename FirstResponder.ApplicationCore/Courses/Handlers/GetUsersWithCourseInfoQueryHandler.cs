using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Courses.DTOs;
using FirstResponder.ApplicationCore.Courses.Queries;
using MediatR;

namespace FirstResponder.ApplicationCore.Courses.Handlers;

public class GetUsersWithCourseInfoQueryHandler : IRequestHandler<GetUsersWithCourseInfoQuery, IEnumerable<UserWithCourseInfoDTO>>
{
    private readonly ICoursesRepository _coursesRepository;
    
    public GetUsersWithCourseInfoQueryHandler(ICoursesRepository coursesRepository)
    {
        _coursesRepository = coursesRepository;
    }
    
    public async Task<IEnumerable<UserWithCourseInfoDTO>> Handle(GetUsersWithCourseInfoQuery request, CancellationToken cancellationToken)
    {
        bool includeNotInCourse = request.Query != "";
        int limitResultsCount = request.Query != "" ? 30 : 0;
		
        return await _coursesRepository.GetUsersWithCourseInfoAsync(request.CourseId, request.Query, limitResultsCount, includeNotInCourse);
    }
}