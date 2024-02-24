using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Common.Exceptions;
using FirstResponder.ApplicationCore.Courses.Commands;
using MediatR;

namespace FirstResponder.ApplicationCore.Courses.Handlers;

public class UpdateCourseTypeCommandHandler : IRequestHandler<UpdateCourseTypeCommand>
{
    private readonly ICourseTypesRepository _courseTypesRepository;
    
    public UpdateCourseTypeCommandHandler(ICourseTypesRepository courseTypesRepository)
    {
        _courseTypesRepository = courseTypesRepository;
    }
    
    public async Task Handle(UpdateCourseTypeCommand request, CancellationToken cancellationToken)
    {
        var courseType = await _courseTypesRepository.GetCourseTypeById(request.CourseTypeId);
        
        if (courseType == null)
        {
            throw new EntityNotFoundException();
        }

        if (courseType.Name == request.Name)
        {
            return;
        }
        
        if (await _courseTypesRepository.CourseTypeExists(request.Name))
        {
            var errors = new Dictionary<string, string>();
            errors["Name"] = "Tento typ školenia už existuje!";
            
            throw new EntityValidationException(errors);
        }
        
        courseType.Name = request.Name;
        
        await _courseTypesRepository.UpdateCourseType(courseType);
    }
}