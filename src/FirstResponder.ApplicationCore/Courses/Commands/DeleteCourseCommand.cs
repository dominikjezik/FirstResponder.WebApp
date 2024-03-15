using MediatR;

namespace FirstResponder.ApplicationCore.Courses.Commands;

public class DeleteCourseCommand : IRequest
{
    public Guid CourseId { get; private set; }

    public DeleteCourseCommand(Guid courseId)
    {
        CourseId = courseId;
    }
}