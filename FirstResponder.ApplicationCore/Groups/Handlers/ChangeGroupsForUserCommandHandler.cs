using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Groups.Commands;
using MediatR;

namespace FirstResponder.ApplicationCore.Groups.Handlers;

public class ChangeGroupsForUserCommandHandler : IRequestHandler<ChangeGroupsForUserCommand>
{
    private readonly IGroupsRepository _groupsRepository;

    public ChangeGroupsForUserCommandHandler(IGroupsRepository groupsRepository)
    {
        _groupsRepository = groupsRepository;
    }

    public async Task Handle(ChangeGroupsForUserCommand request, CancellationToken cancellationToken)
    {
        await _groupsRepository.ChangeGroupsForUser(request.ChangeGroupsForUserDto.UserId, request.ChangeGroupsForUserDto.CheckedOnGroupIds, request.ChangeGroupsForUserDto.CheckedOffGroupIds);
    }
}