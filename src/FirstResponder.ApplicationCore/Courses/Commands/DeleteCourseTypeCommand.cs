using MediatR;

namespace FirstResponder.ApplicationCore.Courses.Commands;

public class DeleteCourseTypeCommand : IRequest
{
    public Guid CourseTypeId { get; set; }

    public DeleteCourseTypeCommand(Guid courseTypeId)
    {
        CourseTypeId = courseTypeId;
    }
}