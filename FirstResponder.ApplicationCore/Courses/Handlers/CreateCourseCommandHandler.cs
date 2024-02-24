using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Common.Exceptions;
using FirstResponder.ApplicationCore.Courses.Commands;
using FirstResponder.ApplicationCore.Entities.CourseAggregate;
using MediatR;

namespace FirstResponder.ApplicationCore.Courses.Handlers;

public class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand, Course>
{
    private readonly ICourseTypesRepository _courseTypesRepository;
    private readonly ICoursesRepository _coursesRepository;
    
    public CreateCourseCommandHandler(ICoursesRepository coursesRepository, ICourseTypesRepository courseTypesRepository)
    {
        _coursesRepository = coursesRepository;
        _courseTypesRepository = courseTypesRepository;
    }
    
    public async Task<Course> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
    {
        var course = request.CourseFormDTO.ToCourse();
        
        // Validate dates
        if (course.StartDate > course.EndDate)
        {
            throw new EntityValidationException("Začiatok školenia nemôže byť neskôr ako jeho koniec.");
        }
        
        // Validate course type
        if (course.CourseTypeId != null)
        {
            var courseType = await _courseTypesRepository.GetCourseTypeById(course.CourseTypeId.Value);
            
            if (courseType == null)
            {
                var errors = new Dictionary<string, string>();
                errors["CourseTypeId"] = "Typ školenia neexistuje";
                
                throw new EntityValidationException(errors);
            }
        }
        
        await _coursesRepository.AddCourse(course);

        return course;
    }
}