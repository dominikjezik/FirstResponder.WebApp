using System.Security.Claims;
using FirstResponder.ApplicationCore.Common.Exceptions;
using FirstResponder.ApplicationCore.Notifications.Commands;
using FirstResponder.ApplicationCore.Notifications.DTOs;
using FirstResponder.ApplicationCore.Notifications.Queries;
using FirstResponder.Web.Extensions;
using FirstResponder.Web.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FirstResponder.Web.Controllers;

[Authorize(Policy = "IsEmployee")]
[Route("users/notifications")]
public class UserNotificationsController  : Controller
{
    private readonly IMediator _mediator;

    public UserNotificationsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [Route("")]
    public async Task<IActionResult> Index()
    {
        return View();
    }
    
    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> Create(NotificationViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(nameof(Index), model);
        }
        
        try
        {
            var senderId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _mediator.Send(new CreateNotificationCommand(model.Content, senderId));
        }
        catch (EntityValidationException exception)
        {
            this.MapErrorsToErrorMessages(exception);
            return RedirectToAction(nameof(Index));
        }
        
        this.DisplaySuccessMessage("Notifikácia bola úspešne vytvorená!");
        return RedirectToAction(nameof(Index));
    }
    
    [HttpPost]
    [Route("update")]
    public async Task<IActionResult> Update(NotificationViewModel model, Guid notificationId)
    {
        if (!ModelState.IsValid)
        {
            return View(nameof(Index), model);
        }
        
        try
        {
            await _mediator.Send(new UpdateNotificationCommand(notificationId, model.Content));
        }
        catch (EntityValidationException exception)
        {
            this.MapErrorsToErrorMessages(exception);
            return RedirectToAction(nameof(Index));
        }
        catch (EntityNotFoundException)
        {
            return NotFound();
        }
        
        
        this.DisplaySuccessMessage("Notifikácia bola úspešne aktualizovaná!");
        return RedirectToAction(nameof(Index));
    }
    
    [HttpPost]
    [Route("delete")]
    public async Task<IActionResult> Delete(Guid notificationId)
    {
        try
        {
            await _mediator.Send(new DeleteNotificationCommand(notificationId));
        }
        catch (EntityNotFoundException)
        {
            return NotFound();
        }
        
        this.DisplaySuccessMessage("Notifikácia bola úspešne vymazaná!");
        return RedirectToAction(nameof(Index));
    }
    
    [HttpGet]
    [Route("filtered-table-items")]
    public async Task<IEnumerable<NotificationDTO>> FilteredTableItems(int pageNumber, [FromQuery] NotificationFiltersDTO filtersDTO)
    {
        return await _mediator.Send(new GetNotificationsQuery() { PageNumber = pageNumber, Filters = filtersDTO });
    }
}