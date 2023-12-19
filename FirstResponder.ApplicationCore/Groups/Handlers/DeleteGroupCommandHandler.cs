using FirstResponder.ApplicationCore.Abstractions;
using FirstResponder.ApplicationCore.Exceptions;
using FirstResponder.ApplicationCore.Groups.Commands;
using MediatR;

namespace FirstResponder.ApplicationCore.Groups.Handlers;

public class DeleteGroupCommandHandler : IRequestHandler<DeleteGroupCommand>
{
	private readonly IGroupsRepository _groupsRepository;

	public DeleteGroupCommandHandler(IGroupsRepository groupsRepository)
	{
		_groupsRepository = groupsRepository;
	}
	
	public async Task Handle(DeleteGroupCommand request, CancellationToken cancellationToken)
	{
		var group = await _groupsRepository.GetGroupById(request.GroupId);
		
		if (group == null)
		{
			throw new EntityNotFoundException();
		}
		
		await _groupsRepository.DeleteGroup(group);
	}
}