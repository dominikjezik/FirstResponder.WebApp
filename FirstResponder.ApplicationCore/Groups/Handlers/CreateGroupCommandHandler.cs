using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Exceptions;
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
		var group = request.GroupFormDto.ToGroup();

		if (await _groupsRepository.GroupExists(group.Name))
		{
			var errors = new Dictionary<string, string>();
			errors["Name"] = "Skupina s týmto názvom už existuje!";
            
			throw new EntityValidationException(errors);
		}

		await _groupsRepository.AddGroup(group);
	}
}