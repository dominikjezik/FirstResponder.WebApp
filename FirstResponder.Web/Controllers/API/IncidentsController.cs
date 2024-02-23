using System.Security.Claims;
using FirstResponder.ApplicationCore.Common.Exceptions;
using FirstResponder.ApplicationCore.Incidents.Commands;
using FirstResponder.ApplicationCore.Incidents.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace FirstResponder.Web.Controllers.API;

[Authorize("Bearer")]
[Authorize("IsResponderOrEmployee")]
[Route("api/[controller]")]
public class IncidentsController : ApiController
{
    private readonly IMediator _mediator;
    
    public IncidentsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    [Route("get-nearby-and-assign")]
    public async Task<IActionResult> GetIncidentsNearby(double latitude, double longitude, double radius)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        
        var incidents = await _mediator.Send(new GetIncidentsNearbyQuery
        {
            Latitude = latitude, 
            Longitude = longitude, 
            Radius = radius,
            UserId = userId
        });

        await _mediator.Send(new AssignResponderToIncidentsCommand
        {
            ResponderId = userId,
            Incidents = incidents
        });
        
        // TODO
        incidents.ToList().ForEach(i =>
        {
            i.Responders.ToList().ForEach(r =>
            {
                r.Incident = null;
                r.Responder = null;
            });
        });
        
        return Ok(incidents);
    }
    
    [HttpPost]
    [Route("{incidentId}/accept")]
    public async Task<IActionResult> AcceptIncident(Guid incidentId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        try
        {
            await _mediator.Send(new AcceptIncidentCommand
            {
                IncidentId = incidentId,
                ResponderId = userId
            });
        }
        catch (EntityNotFoundException)
        {
            return NotFound();
        }
        catch (EntityValidationException e)
        {
            return BadRequest(e.Message);
        }

        // TODO: Vratit incident details?

        return Ok();
    }
    
    [HttpPost]
    [Route("{incidentId}/decline")]
    public async Task<IActionResult> DeclineIncident(Guid incidentId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        try
        {
            await _mediator.Send(new DeclineIncidentCommand
            {
                IncidentId = incidentId,
                ResponderId = userId
            });
        }
        catch (EntityNotFoundException)
        {
            return NotFound();
        }
        catch (EntityValidationException e)
        {
            return BadRequest(e.Message);
        }

        return Ok();
    }
    
}