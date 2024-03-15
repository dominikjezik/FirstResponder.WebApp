using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Common.Exceptions;
using FirstResponder.ApplicationCore.Courses.Commands;
using FirstResponder.ApplicationCore.Entities.CourseAggregate;
using MediatR;

namespace FirstResponder.ApplicationCore.Courses.Handlers;

public class CreateCourseTypeCommandHandler : IRequestHandler<CreateCourseTypeCommand>
{
    private readonly ICourseTypesRepository _courseTypesRepository;
    
    public CreateCourseTypeCommandHandler(ICourseTypesRepository courseTypesRepository)
    {
        _courseTypesRepository = courseTypesRepository;
    }
    
    public async Task Handle(CreateCourseTypeCommand request, CancellationToken cancellationToken)
    {
        var courseType = new CourseType()
        {
            Name = request.Name
        };
        
        if (await _courseTypesRepository.CourseTypeExists(courseType.Name))
        {
            throw new EntityValidationException("Name", "Tento typ školenia už existuje!");
        }
        
        await _courseTypesRepository.AddCourseType(courseType);
    }
}