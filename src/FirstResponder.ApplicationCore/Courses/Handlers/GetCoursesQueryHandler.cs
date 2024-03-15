using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Courses.Queries;
using FirstResponder.ApplicationCore.Entities.CourseAggregate;
using MediatR;

namespace FirstResponder.ApplicationCore.Courses.Handlers;

public class GetCoursesQueryHandler : IRequestHandler<GetCoursesQuery, IEnumerable<Course>>
{
    private readonly ICoursesRepository _coursesRepository;
    private const int PageSize = 30;
    
    public GetCoursesQueryHandler(ICoursesRepository coursesRepository)
    {
        _coursesRepository = coursesRepository;
    }
    
    public async Task<IEnumerable<Course>> Handle(GetCoursesQuery request, CancellationToken cancellationToken)
    {
        int pageNumber = request.PageNumber;

        if (pageNumber < 0)
        {
            pageNumber = 0;
        }

        return await _coursesRepository.GetCourses(pageNumber, PageSize, request.Filters);
    }
}