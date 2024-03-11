using System.Security.Claims;
using FirstResponder.ApplicationCore.Aeds.DTOs;
using FirstResponder.ApplicationCore.Aeds.Queries;
using FirstResponder.ApplicationCore.Entities.AedAggregate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FirstResponder.Web.Controllers.API;

[Authorize("Bearer")]
[Route("api/[controller]")]
public class AedController : ApiController
{
    private readonly IMediator _mediator;

    public AedController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    [Authorize("IsEmployee")]
    [Route("")]
    public async Task<IEnumerable<AedItemDTO>> GetAeds([FromQuery] int pageNumber, [FromQuery] AedItemFiltersDTO filtersDTO)
    {
        return await _mediator.Send(new GetAedItemsQuery() { PageNumber = pageNumber, Filters = filtersDTO });
    }
    
    [HttpGet]
    [Authorize("IsResponderOrEmployee")]
    [Route("my-aeds")]
    public async Task<IActionResult> GetMyAeds()
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
    [Authorize("IsResponderOrEmployee")]
    [Route("my-aeds/{aedId}")]
    public async Task<IActionResult> GetMyAed(string aedId)
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
    
    
}