using MediatR;

namespace FirstResponder.ApplicationCore.Groups.Commands;

public class DeleteGroupCommand : IRequest
{
	public Guid GroupId { get; private set; }
	
	public DeleteGroupCommand(Guid groupId)
	{
		GroupId = groupId;
	}
}