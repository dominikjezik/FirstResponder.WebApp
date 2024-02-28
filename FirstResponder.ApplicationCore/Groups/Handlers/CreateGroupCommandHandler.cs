using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Common.Exceptions;
using FirstResponder.ApplicationCore.Groups.Commands;
using MediatR;

namespace FirstResponder.ApplicationCore.Groups.Handlers;

public class CreateGroupCommandHandler : IRequestHandler<CreateGroupCommand>
{
	private readonly IGroupsRepository _groupsRepository;

	public CreateGroupCommandHandler(IGroupsRepository groupsRepository)
	{
		_groupsRepository = groupsRepository;
	}
	
	public async Task Handle(CreateGroupCommand request, CancellationToken cancellationToken)
	{
		var group = request.GroupFormDTO.ToGroup();

		if (await _groupsRepository.GroupExists(group.Name))
		{
			throw new EntityValidationException("Name", "Skupina s týmto názvom už existuje!");
		}

		await _groupsRepository.AddGroup(group);
	}
}