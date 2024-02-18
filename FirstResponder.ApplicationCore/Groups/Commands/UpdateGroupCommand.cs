using FirstResponder.ApplicationCore.Groups.DTOs;
using MediatR;

namespace FirstResponder.ApplicationCore.Groups.Commands;

public class UpdateGroupCommand : IRequest
{
	public GroupFormDTO GroupFormDTO { get; private set; }

	public UpdateGroupCommand(GroupFormDTO groupFormDto)
	{
		GroupFormDTO = groupFormDto;
	}
}