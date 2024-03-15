using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Courses.Queries;
using FirstResponder.ApplicationCore.Entities.CourseAggregate;
using MediatR;

namespace FirstResponder.ApplicationCore.Courses.Handlers;

public class GetAllCourseTypesQueryHandler : IRequestHandler<GetAllCourseTypesQuery, IEnumerable<CourseType>>
{
    private readonly ICourseTypesRepository _courseTypesRepository;
    
    public GetAllCourseTypesQueryHandler(ICourseTypesRepository courseTypesRepository)
    {
        _courseTypesRepository = courseTypesRepository;
    }
    
    public async Task<IEnumerable<CourseType>> Handle(GetAllCourseTypesQuery request, CancellationToken cancellationToken)
    {
        return await _courseTypesRepository.GetAllCourseTypes();
    }
}