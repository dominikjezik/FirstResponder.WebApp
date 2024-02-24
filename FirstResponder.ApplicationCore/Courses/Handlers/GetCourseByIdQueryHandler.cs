using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Courses.DTOs;
using FirstResponder.ApplicationCore.Courses.Queries;
using MediatR;

namespace FirstResponder.ApplicationCore.Courses.Handlers;

public class GetCourseByIdQueryHandler : IRequestHandler<GetCourseByIdQuery, CourseDTO?>
{
    private readonly ICoursesRepository _coursesRepository;
    
    public GetCourseByIdQueryHandler(ICoursesRepository coursesRepository)
    {
        _coursesRepository = coursesRepository;
    }
    
    public async Task<CourseDTO?> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
    {
        return await _coursesRepository.GetCourseDetailsById(request.CourseId);
    }
}