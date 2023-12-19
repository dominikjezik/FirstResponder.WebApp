using FirstResponder.ApplicationCore.Groups.DTOs;
using MediatR;

namespace FirstResponder.ApplicationCore.Groups.Commands;

public class CreateGroupCommand : IRequest
{
	public GroupFormDTO GroupFormDto { get; private set; }

	public CreateGroupCommand(GroupFormDTO groupFormDto)
	{ 
		GroupFormDto = groupFormDto;
	}
}