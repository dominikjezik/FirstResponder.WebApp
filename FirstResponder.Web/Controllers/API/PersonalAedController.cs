using System.Security.Claims;
using FirstResponder.ApplicationCore.Aeds.Commands;
using FirstResponder.ApplicationCore.Aeds.DTOs;
using FirstResponder.ApplicationCore.Aeds.Queries;
using FirstResponder.ApplicationCore.Common.Exceptions;
using FirstResponder.ApplicationCore.Entities.AedAggregate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FirstResponder.Web.Controllers.API;

[Authorize("Bearer")]
[Authorize("IsResponderOrEmployee")]
[Route("api/personal-aed")]
public class PersonalAedController : ApiController
{
    private readonly IMediator _mediator;
    
    public PersonalAedController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetUserPersonalAeds()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userId, out var userGuid))
        {
            return BadRequest();
        }

        var aeds = await _mediator.Send(new GetPersonalAedItemsByOwnerQuery(userGuid));
        
        return Ok(aeds);
    }
    
    [HttpGet]
    [Route("{aedId}")]
    public async Task<IActionResult> Details(string aedId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userId, out var userGuid))
        {
            return BadRequest();
        }

        var aed = await _mediator.Send(new GetAedByIdQuery(aedId));
        
        if (aed == null)
        {
            return NotFound();
        }
        
        if (aed is not PersonalAed personalAed || personalAed.OwnerId != userGuid)
        {
            return Unauthorized();
        }
        
        return Ok(personalAed);
    }
    
    [HttpPost]
    [Route("")]
    public async Task<IActionResult> Create([FromBody] AedFormDTO model)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userId, out var userGuid))
        {
            return BadRequest();
        }

        try
        {
            var aed = await _mediator.Send(new CreatePersonalAedCommand(model, userGuid));
            return CreatedAtAction(nameof(Details), new { aedId = aed.Id });
        }
        catch (EntityValidationException exception)
        {
            return BadRequest(exception.ValidationErrors);
        }
    }
    
    [HttpPut]
    [Route("{aedId}")]
    public async Task<IActionResult> Update(string aedId, [FromBody] AedFormDTO model)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userId, out var userGuid))
        {
            return BadRequest();
        }

        try
        {
            var aed = await _mediator.Send(new GetAedByIdQuery(aedId));
            
            if (aed == null)
            {
                return NotFound();
            }
            
            model.AedId = aed.Id;
            await _mediator.Send(new UpdatePersonalAedCommand(model, userGuid));
            return Ok();
        }
        catch (EntityValidationException exception)
        {
            return BadRequest(exception.ValidationErrors);
        }
        catch (UnauthorizedException)
        {
            return Unauthorized();
        }
        catch (EntityNotFoundException)
        {
            return NotFound();
        }
    }
    
    [HttpDelete]
    [Route("{aedId}")]
    public async Task<IActionResult> Delete(string aedId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userId, out var userGuid))
        {
            return BadRequest();
        }

        var aed = await _mediator.Send(new GetAedByIdQuery(aedId));
        
        if (aed == null)
        {
            return NotFound();
        }

        try
        {
            await _mediator.Send(new DeletePersonalAedCommand(aedId, userGuid));
        }
        catch (UnauthorizedException)
        {
            return Unauthorized();
        }
        catch (EntityNotFoundException)
        {
            return NotFound();
        }
        
        return Ok();
    }
}