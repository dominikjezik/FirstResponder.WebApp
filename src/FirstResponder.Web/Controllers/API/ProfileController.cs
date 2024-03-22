using System.Security.Claims;
using FirstResponder.ApplicationCore.Common.Exceptions;
using FirstResponder.ApplicationCore.Users.Commands;
using FirstResponder.ApplicationCore.Users.DTOs;
using FirstResponder.ApplicationCore.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FirstResponder.Web.Controllers.API;

[Authorize("Bearer")]
[Route("api/[controller]")]
public class ProfileController : ApiController
{
    private readonly IMediator _mediator;

    public ProfileController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    [Authorize("Bearer")]
    [Route("")]
    public async Task<IActionResult> GetProfile()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var user = await _mediator.Send(new GetUserProfileByIdQuery(userId));
        
        if (user == null)
        {
            return NotFound();
        }
        
        return Ok(user);
    }
    
    [HttpPut]
    [Authorize("Bearer")]
    [Route("")]
    public async Task<IActionResult> UpdateProfile(UserProfileDTO model)
    {
        var validGuid = Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId);
        
        if (!validGuid)
        {
            return BadRequest("Invalid user id");
        }

        try
        {
            await _mediator.Send(new UpdateUserCommand(userId, model));
        }
        catch(EntityNotFoundException)
        {
            return NotFound();
        }
        catch (EntityValidationException exception)
        {
            return BadRequest(exception.ValidationErrors);
        }
        
        return Ok();
    }
}