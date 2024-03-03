using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Courses.Commands;
using MediatR;

namespace FirstResponder.ApplicationCore.Courses.Handlers;

public class ChangeUsersInCourseCommandHandler : IRequestHandler<ChangeUsersInCourseCommand>
{
    private readonly ICoursesRepository _coursesRepository;
    
    public ChangeUsersInCourseCommandHandler(ICoursesRepository coursesRepository)
    {
        _coursesRepository = coursesRepository;
    }
    
    public async Task Handle(ChangeUsersInCourseCommand request, CancellationToken cancellationToken)
    {
        await _coursesRepository.ChangeUsersInCourse(request.UsersAssociationChangeDTO.EntityId, request.UsersAssociationChangeDTO.CheckedOnUserIds, request.UsersAssociationChangeDTO.CheckedOffUserIds);
    }
}