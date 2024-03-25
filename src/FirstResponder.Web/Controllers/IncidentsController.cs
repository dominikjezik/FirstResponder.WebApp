using System.Security.Claims;
using FirstResponder.ApplicationCore.Common.Enums;
using FirstResponder.ApplicationCore.Common.Exceptions;
using FirstResponder.ApplicationCore.Incidents.Commands;
using FirstResponder.ApplicationCore.Incidents.DTOs;
using FirstResponder.ApplicationCore.Incidents.Queries;
using FirstResponder.Web.Extensions;
using FirstResponder.Web.Hubs;
using FirstResponder.Web.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace FirstResponder.Web.Controllers;

[Authorize(Policy = "IsEmployee")]
[Route("[controller]")]
public class IncidentsController : Controller
{
    private readonly IMediator _mediator;
    private readonly IHubContext<IncidentsHub> _hubContext;

    public IncidentsController(IMediator mediator, IHubContext<IncidentsHub> hubContext)
    {
        _mediator = mediator;
        _hubContext = hubContext;
    }
    
    [Route("")]
    public IActionResult Index()
    {
        return View();
    }
    
    [Route("[action]")]
    public async Task<IActionResult> Map()
    {
        return View();
    }
    
    [Route("[action]")]
    public async Task<IActionResult> Calendar()
    {
        return View();
    }
    
    [Route("[action]")]
    public async Task<IActionResult> Create()
    {
        return View();
    }
    
    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> Create(IncidentFormDTO model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        
        try
        {
            var incident = await _mediator.Send(new CreateIncidentCommand(model));
            this.DisplaySuccessMessage("Zásah bol úspešne vytvorený!");
            return RedirectToAction(nameof(Edit), new { incidentId = incident.Id });
        }
        catch (EntityValidationException exception)
        {
            this.MapErrorsToModelState(exception);
            return View(model);
        }
    }
    
    [Route("{incidentId}")]
    public async Task<IActionResult> Edit(string incidentId)
    {
        var incident = await _mediator.Send(new GetIncidentByIdQuery(incidentId));

        if (incident == null)
        {
            return NotFound();
        }
        
        return View(incident);
    }
    
    [HttpPost]
    [Route("{incidentId}")]
    public async Task<IActionResult> Edit(string incidentId, IncidentDTO model)
    {
        var incident = await _mediator.Send(new GetIncidentByIdQuery(incidentId));
        
        if (incident == null)
        {
            return NotFound();
        }
        
        if (!ModelState.IsValid)
        {
            // Ensure that the original data remains filled in
            incident.IncidentForm = model.IncidentForm;
            return View(incident);
        }
        
        try
        {
            await _mediator.Send(new UpdateIncidentCommand(incident.IncidentId, model.IncidentForm));
            this.DisplaySuccessMessage("Zásah bol úspešne aktualizovaný!");
            return RedirectToAction(nameof(Edit), new { incidentId = incident.IncidentId });
        }
        catch (EntityNotFoundException)
        {
            return NotFound();
        }
        catch (EntityValidationException exception)
        {
            this.MapErrorsToModelState(exception);
            // Ensure that the original data remains filled in
            incident.IncidentForm = model.IncidentForm;
            return View(incident);
        }
    }
    
    [HttpPost]
    [Route("{incidentId}/[action]")]
    public async Task<IActionResult> Delete(Guid incidentId)
    {
        try
        {
            await _mediator.Send(new DeleteIncidentCommand(incidentId));
            this.DisplaySuccessMessage("Zásah bol úspešne vymazaný!");
            return RedirectToAction(nameof(Index));
        }
        catch (EntityNotFoundException)
        {
            return NotFound();
        }
    }
    
    [HttpPost]
    [Route("{incidentId}/[action]")]
    public async Task<IActionResult> Close(Guid incidentId)
    {
        try
        {
            var incident = await _mediator.Send(new CloseIncidentCommand(incidentId));
            
            if (incident.State == IncidentState.Canceled)
            {
                this.DisplaySuccessMessage("Zásah bol úspešne zrušený!");
            }
            else
            {
                this.DisplaySuccessMessage("Zásah bol úspešne ukončený!");
            }
            
            return RedirectToAction(nameof(Edit), new { incidentId });
        }
        catch (EntityNotFoundException)
        {
            return NotFound();
        }
    }
    
    [HttpPost]
    [Route("{incidentId}/[action]")]
    public async Task<IActionResult> SearchAndNotifyResponders(Guid incidentId)
    {
        try
        {
            await _mediator.Send(new RequestDeviceLocationsCommand());
            this.DisplaySuccessMessage("Dostupní responderi v okolí boli úspešne upozornení!");
            return RedirectToAction(nameof(Edit), new { incidentId });
        }
        catch (EntityNotFoundException)
        {
            return NotFound();
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            this.DisplayErrorMessage("Nastala chyba pri upozornení responderov!");
            return RedirectToAction(nameof(Edit), new { incidentId });
        }
    }
    
    [HttpGet]
    [Route("{incidentId}/responders")]
    public async Task<IEnumerable<IncidentResponderItemDTO>> Responders(Guid incidentId)
    {
        var responders = await _mediator.Send(new GetIncidentRespondersQuery(incidentId));
        return responders;
    }
    
    [HttpGet]
    [Route("{incidentId}/reports/{responderId}")]
    public async Task<IActionResult> Report(Guid incidentId, Guid responderId)
    {
        var report = await _mediator.Send(new GetIncidentResponderReportQuery(incidentId, responderId));
        
        if (report == null)
        {
            return NotFound();
        }
        
        return Ok(report);
    }
    
    [HttpGet]
    [Route("{incidentId}/messages")]
    public async Task<IEnumerable<IncidentMessageDTO>> Messages(Guid incidentId)
    {
        var messages = await _mediator.Send(new GetIncidentMessagesQuery(incidentId));
        return messages;
    }
    
    [HttpPost]
    [IgnoreAntiforgeryToken]
    [Route("{incidentId}/messages")]
    public async Task<IActionResult> SendMessage(Guid incidentId, [FromBody] IncidentNewMessageViewModel viewModel)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var message = await _mediator.Send(new CreateIncidentMessageCommand(incidentId, userId, viewModel.MessageContent));
            
            // Update messages on edit page (SignalR) (possibly other employee)
            await _hubContext.Clients.Group(incidentId.ToString()).SendAsync("NewMessage", message);
            
            return Ok(message);
        }
        catch (EntityValidationException exception)
        {
            return BadRequest(exception.ValidationErrors);
        }
        catch (EntityNotFoundException)
        {
            return BadRequest();
        }
    }
    
    [HttpGet]
    [Route("filtered-table-items")]
    public async Task<IEnumerable<IncidentItemDTO>> FilteredTableItems([FromQuery] IncidentItemFiltersDTO filtersDto, int pageNumber = 0, int pageSize = 0)
    {
        var items = await _mediator.Send(new GetIncidentItemsQuery { PageNumber = pageNumber, PageSize = pageSize, Filters = filtersDto });
        return items;
    }
}