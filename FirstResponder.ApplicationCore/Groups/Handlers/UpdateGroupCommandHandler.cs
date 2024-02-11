using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Common.Exceptions;
using FirstResponder.ApplicationCore.Exceptions;
using FirstResponder.ApplicationCore.Groups.Commands;
using MediatR;

namespace FirstResponder.ApplicationCore.Groups.Handlers;

public class UpdateGroupCommandHandler : IRequestHandler<UpdateGroupCommand>
{
	private readonly IGroupsRepository _groupsRepository;

	public UpdateGroupCommandHandler(IGroupsRepository groupsRepository)
	{
		_groupsRepository = groupsRepository;
	}
	
	public async Task Handle(UpdateGroupCommand request, CancellationToken cancellationToken)
	{
		var group = await _groupsRepository.GetGroupById(request.GroupFormDto.GroupId);
		
		if (group == null)
		{
			throw new EntityNotFoundException();
		}
		
		string oldName = group.Name;
		
		if (oldName != request.GroupFormDto.Name && await _groupsRepository.GroupExists(request.GroupFormDto.Name))
		{
			var errors = new Dictionary<string, string>();
			errors["Name"] = "Skupina s týmto názvom už existuje!";
				
			throw new EntityValidationException(errors);
		}

		request.GroupFormDto.ToGroup(group);
		
		await _groupsRepository.UpdateGroup(group);
	}
}