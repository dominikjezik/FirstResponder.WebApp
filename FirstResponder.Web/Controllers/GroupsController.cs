using FirstResponder.ApplicationCore.Exceptions;
using FirstResponder.ApplicationCore.Groups.Commands;
using FirstResponder.ApplicationCore.Groups.DTOs;
using FirstResponder.ApplicationCore.Groups.Queries;
using FirstResponder.Web.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FirstResponder.Web.Controllers;

[Authorize(Policy = "IsEmployee")]
[Route("[controller]")]
public class GroupsController : Controller
{
	private readonly IMediator _mediator;

	public GroupsController(IMediator mediator)
	{
		_mediator = mediator;
	}
	
	[Route("")]
	public async Task<IActionResult> Index()
	{
		var groups = await _mediator.Send(new GetAllGroupsQuery());
		return View(groups);
	}

	[HttpPost]
	[Route("create")]
	public async Task<IActionResult> Create(GroupFormDTO model)
	{
		if (!ModelState.IsValid)
		{
			var groups = await _mediator.Send(new GetAllGroupsQuery());
			return View("Index", groups);
		}
		
		try
		{
			await _mediator.Send(new CreateGroupCommand(model));
		}
		catch (EntityValidationException exception)
		{
			this.MapErrorsToErrorMessages(exception);
			return RedirectToAction(nameof(Index));
		}
        
		this.DisplaySuccessMessage("Skupina " + model.Name + " bola úspešne vytvorená!");
		return RedirectToAction(nameof(Index));
	}
	
	[HttpPost]
	[Route("update")]
	public async Task<IActionResult> Update(Guid groupId, GroupFormDTO model)
	{
		if (!ModelState.IsValid)
		{
			var groups = await _mediator.Send(new GetAllGroupsQuery());
			return View("Index", groups);
		}
		
		try
		{
			await _mediator.Send(new UpdateGroupCommand(model));
		}
		catch (EntityValidationException exception)
		{
			this.MapErrorsToErrorMessages(exception);
			return RedirectToAction(nameof(Index));
		}
		
		this.DisplaySuccessMessage("Skupina " + model.Name + " bola úspešne upravená!");
		return RedirectToAction(nameof(Index));
	}
	
	[HttpPost]
	[Route("delete")]
	public async Task<IActionResult> Delete(Guid groupId)
	{
		try
		{
			await _mediator.Send(new DeleteGroupCommand(groupId));
		}
		catch (EntityValidationException exception)
		{
			this.MapErrorsToErrorMessages(exception);
			return RedirectToAction(nameof(Index));
		}
		
		this.DisplaySuccessMessage("Skupina bola úspešne odstránená!");
		return RedirectToAction(nameof(Index));
	}
	
	[HttpGet]
	[Route("{groupId}/users")]
	public async Task<IEnumerable<UserWithGroupInfoDTO>> Users(Guid groupId, string query = "")
	{
		var users = await _mediator.Send(new GetUsersWithGroupInfoQuery(groupId, query));
		return users;
	}

	[HttpPost]
	[IgnoreAntiforgeryToken]
	[Route("{groupId}/users")]
	public async Task<IActionResult> ChangeUsers([FromBody] ChangeUsersInGroupDTO model)
	{
		await _mediator.Send(new ChangeUsersInGroupCommand(model));
		return Ok();
	}
}