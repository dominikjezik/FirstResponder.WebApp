using FirstResponder.ApplicationCore.Groups.Commands;
using FirstResponder.ApplicationCore.Groups.DTOs;
using FirstResponder.ApplicationCore.Groups.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FirstResponder.Web.Controllers;

[Authorize(Policy = "IsEmployee")]
public class UserGroupsController  : Controller
{
    private readonly IMediator _mediator;

    public UserGroupsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    [Route("users/{userId}/groups")]
    public async Task<IEnumerable<GroupWithUserInfoDTO>> Groups(Guid userId, string query = "")
    {
        var groups = await _mediator.Send(new GetGroupsWithUserInfoQuery(userId, query));
        return groups;
    }

    [HttpPost]
    [IgnoreAntiforgeryToken]
    [Route("users/{userId}/groups")]
    public async Task<IActionResult> ChangeGroups([FromBody] ChangeGroupsForUserDTO model)
    {
        await _mediator.Send(new ChangeGroupsForUserCommand(model));
        return Ok();
    }
    
}