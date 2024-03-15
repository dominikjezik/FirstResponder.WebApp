using MediatR;

namespace FirstResponder.ApplicationCore.Courses.Commands;

public class CreateCourseTypeCommand : IRequest
{
    public string Name { get; private set; }

    public CreateCourseTypeCommand(string name)
    {
        Name = name;
    }
}