using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Common.Exceptions;
using FirstResponder.ApplicationCore.Courses.Commands;
using FirstResponder.ApplicationCore.Entities.CourseAggregate;
using MediatR;

namespace FirstResponder.ApplicationCore.Courses.Handlers;

public class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand, Course>
{
    private readonly ICoursesRepository _coursesRepository;
    
    public CreateCourseCommandHandler(ICoursesRepository coursesRepository)
    {
        _coursesRepository = coursesRepository;
    }
    
    public async Task<Course> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
    {
        var course = request.CourseFormDTO.ToCourse();
        
        // Validate dates
        if (course.StartDate > course.EndDate)
        {
            throw new EntityValidationException("Začiatok školenia nemôže byť neskôr ako jeho koniec.");
        }
        
        await _coursesRepository.AddCourse(course);

        return course;
    }
}