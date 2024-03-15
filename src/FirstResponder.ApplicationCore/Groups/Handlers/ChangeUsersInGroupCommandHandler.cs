using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Groups.Commands;
using MediatR;

namespace FirstResponder.ApplicationCore.Groups.Handlers;

public class ChangeUsersInGroupCommandHandler : IRequestHandler<ChangeUsersInGroupCommand>
{
    private readonly IGroupsRepository _groupsRepository;

    public ChangeUsersInGroupCommandHandler(IGroupsRepository groupsRepository)
    {
        _groupsRepository = groupsRepository;
    }
    
    public async Task Handle(ChangeUsersInGroupCommand request, CancellationToken cancellationToken)
    {
        await _groupsRepository.ChangeUsersInGroup(request.UsersAssociationChangeDTO.EntityId, request.UsersAssociationChangeDTO.CheckedOnUserIds, request.UsersAssociationChangeDTO.CheckedOffUserIds);
    }
}