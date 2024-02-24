using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Common.Exceptions;
using FirstResponder.ApplicationCore.Courses.Commands;
using MediatR;

namespace FirstResponder.ApplicationCore.Courses.Handlers;

public class DeleteCourseCommandHandler : IRequestHandler<DeleteCourseCommand>
{
    private readonly ICoursesRepository _coursesRepository;
    
    public DeleteCourseCommandHandler(ICoursesRepository coursesRepository)
    {
        _coursesRepository = coursesRepository;
    }
    
    public async Task Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
    {
        var course = await _coursesRepository.GetCourseById(request.CourseId);
        
        if (course == null)
        {
            throw new EntityNotFoundException();
        }
        
        await _coursesRepository.DeleteCourse(course);
    }
}