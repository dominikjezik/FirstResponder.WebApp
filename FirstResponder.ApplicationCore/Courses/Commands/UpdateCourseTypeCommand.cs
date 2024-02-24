using MediatR;

namespace FirstResponder.ApplicationCore.Courses.Commands;

public class UpdateCourseTypeCommand : IRequest
{
    public Guid CourseTypeId { get; private set; }
    
    public string Name { get; private set; }

    public UpdateCourseTypeCommand(Guid courseTypeId, string name)
    {
        CourseTypeId = courseTypeId;
        Name = name;
    }
}