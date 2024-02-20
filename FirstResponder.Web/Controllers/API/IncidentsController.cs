using System.Security.Claims;
using FirstResponder.ApplicationCore.Incidents.Commands;
using FirstResponder.ApplicationCore.Incidents.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
}