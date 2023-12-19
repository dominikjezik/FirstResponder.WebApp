using FirstResponder.ApplicationCore.Groups.DTOs;
using MediatR;

namespace FirstResponder.ApplicationCore.Groups.Commands;

public class UpdateGroupCommand : IRequest
{
	public GroupFormDTO GroupFormDto { get; set; }

	public UpdateGroupCommand(GroupFormDTO groupFormDto)
	{
		GroupFormDto = groupFormDto;
	}
}