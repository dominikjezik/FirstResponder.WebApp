using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Common.Exceptions;
using FirstResponder.ApplicationCore.Courses.Commands;
using FirstResponder.ApplicationCore.Entities.CourseAggregate;
using MediatR;

namespace FirstResponder.ApplicationCore.Courses.Handlers;

public class UpdateCourseCommandHandler : IRequestHandler<UpdateCourseCommand, Course?>
{
    private readonly ICoursesRepository _coursesRepository;
    private readonly ICourseTypesRepository _courseTypesRepository;

    public UpdateCourseCommandHandler(ICoursesRepository coursesRepository, ICourseTypesRepository courseTypesRepository)
    {
        _coursesRepository = coursesRepository;
        _courseTypesRepository = courseTypesRepository;
    }

    public async Task<Course?> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
    {
        var course = await _coursesRepository.GetCourseById(request.CourseId);
        
        if (course == null)
        {
            throw new EntityNotFoundException();
        }
        
        request.CourseFormDTO.ToCourse(course);
        
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
                throw new EntityValidationException("CourseTypeId", "Typ školenia neexistuje");
            }
        }
        
        await _coursesRepository.UpdateCourse(course);
        return course;
    }
}