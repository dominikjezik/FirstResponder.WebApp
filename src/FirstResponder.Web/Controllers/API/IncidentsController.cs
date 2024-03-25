using System.Security.Claims;
using FirstResponder.ApplicationCore.Common.Enums;
using FirstResponder.ApplicationCore.Common.Exceptions;
using FirstResponder.ApplicationCore.Incidents.Commands;
using FirstResponder.ApplicationCore.Incidents.DTOs;
using FirstResponder.ApplicationCore.Incidents.Queries;
using FirstResponder.Web.Hubs;
using FirstResponder.Web.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace FirstResponder.Web.Controllers.API;

[Authorize("Bearer")]
[Authorize("IsResponderOrEmployee")]
[Route("api/[controller]")]
public class IncidentsController : ApiController
{
    private readonly IMediator _mediator;
    private readonly IHubContext<IncidentsHub> _hubContext;

    public IncidentsController(IMediator mediator, IHubContext<IncidentsHub> hubContext)
    {
        _mediator = mediator;
        _hubContext = hubContext;
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

        var incidentItems = incidents.Select(i => i.ToItemForResponderDTO(i.Responders.FirstOrDefault()));
        
        return Ok(incidentItems);
    }
    
    [HttpGet]
    [Route("my-list")]
    public async Task<IActionResult> GetMyIncidents()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (!Guid.TryParse(userId, out var userGuid))
        {
            return BadRequest();
        }

        var incidents = await _mediator.Send(new GetIncidentsForResponderRequest(userGuid));
        
        return Ok(incidents);
    }
    
    [HttpPost]
    [Route("{incidentId}/accept")]
    public async Task<IActionResult> AcceptIncident(Guid incidentId, double? latitude = null, double? longitude = null, TypeOfResponderTransport? typeOfTransport = null)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        try
        {
            var incidentResponder = await _mediator.Send(new AcceptIncidentCommand
            {
                IncidentId = incidentId,
                ResponderId = userId,
                Latitude = latitude,
                Longitude = longitude,
                TypeOfTransport = typeOfTransport
            });
            
            // Update responders list on edit page (SignalR)
            await _hubContext.Clients.Group(incidentId.ToString()).SendAsync("ResponderAccepted", incidentResponder);
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
            
            //  Update responders list on edit page (SignalR)
            await _hubContext.Clients.Group(incidentId.ToString()).SendAsync("ResponderDeclined", userId);
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
    
    [HttpPost]
    [Route("{incidentId}/update-location")]
    public async Task<IActionResult> UpdateLocation(Guid incidentId, double latitude, double longitude, TypeOfResponderTransport? typeOfTransport = null)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        try
        {
            var incidentResponder = await _mediator.Send(new UpdateResponderLocationCommand
            {
                IncidentId = incidentId,
                ResponderId = userId,
                Latitude = latitude,
                Longitude = longitude,
                TypeOfTransport = typeOfTransport
            });
            
            if (incidentResponder == null)
            {
                return NotFound();
            }
            
            // Update responder item on edit page (SignalR)
            await _hubContext.Clients.Group(incidentId.ToString()).SendAsync("ResponderLocationChanged", incidentResponder);
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
    
    [HttpGet]
    [Route("{incidentId}")]
    public async Task<IActionResult> GetIncident(Guid incidentId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (!Guid.TryParse(userId, out var userGuid))
        {
            return BadRequest();
        }

        try
        {
            var incident = await _mediator.Send(new GetIncidentForResponderRequest(incidentId, userGuid));
            
            if (incident == null)
            {
                return NotFound();
            }
            
            return Ok(incident);
        }
        catch (UnauthorizedException e)
        {
            return Unauthorized(e.Message);
        }
    }
    
    [HttpPost]
    [Route("{incidentId}/messages")]
    public async Task<IActionResult> SendMessage(Guid incidentId, IncidentNewMessageViewModel model)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        try
        {
            var message = await _mediator.Send(new CreateIncidentMessageCommand(incidentId, userId, model.MessageContent, true));
            
            // Update messages on edit page (SignalR)
            await _hubContext.Clients.Group(incidentId.ToString()).SendAsync("NewMessage", message);
            
            return Ok(message);
        }
        catch (EntityNotFoundException)
        {
            return NotFound();
        }
        catch (EntityValidationException e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpGet]
    [Route("{incidentId}/report")]
    public async Task<IActionResult> GetReport(Guid incidentId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (!Guid.TryParse(userId, out var userGuid))
        {
            return BadRequest();
        }
        
        var report = await _mediator.Send(new GetIncidentResponderReportQuery(incidentId, userGuid));
        
        if (report == null)
        {
            return NotFound();
        }
        
        return Ok(report);
    }
    
    [HttpPost]
    [Route("{incidentId}/report")]
    public async Task<IActionResult> StoreReport(Guid incidentId, [FromBody] IncidentReportFormDTO model)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        try
        {
            await _mediator.Send(new StoreIncidentReportCommand
            {
                IncidentId = incidentId,
                ResponderId = userId,
                Report = model
            });
            
            // Update responder item on edit page (SignalR)
            await _hubContext.Clients.Group(incidentId.ToString()).SendAsync("ResponderReportSubmitted", userId);
        }
        catch (EntityNotFoundException e)
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