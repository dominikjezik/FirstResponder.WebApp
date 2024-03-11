using System.Security.Claims;
using FirstResponder.ApplicationCore.Notifications.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FirstResponder.Web.Controllers.API;

[Route("api/[controller]")]
public class NotificationsController : ApiController
{
    private readonly IMediator _mediator;
    
    public NotificationsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    [Authorize("Bearer")]
    [Authorize("IsResponderOrEmployee")]
    [Route("")]
    public async Task<IActionResult> GetNotifications()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        var notifications = await _mediator.Send(new GetUserNotificationsQuery(userId));
        
        return Ok(notifications);
    }
}