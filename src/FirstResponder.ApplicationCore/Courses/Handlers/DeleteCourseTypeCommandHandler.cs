using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Common.Exceptions;
using FirstResponder.ApplicationCore.Courses.Commands;
using MediatR;

namespace FirstResponder.ApplicationCore.Courses.Handlers;

public class DeleteCourseTypeCommandHandler : IRequestHandler<DeleteCourseTypeCommand>
{
    private readonly ICourseTypesRepository _courseTypesRepository;
    
    public DeleteCourseTypeCommandHandler(ICourseTypesRepository courseTypesRepository)
    {
        _courseTypesRepository = courseTypesRepository;
    }
    
    public async Task Handle(DeleteCourseTypeCommand request, CancellationToken cancellationToken)
    {
        var courseType = await _courseTypesRepository.GetCourseTypeById(request.CourseTypeId);
        
        if (courseType == null)
        {
            throw new EntityNotFoundException();
        }
        
        await _courseTypesRepository.DeleteCourseType(courseType);
    }
}