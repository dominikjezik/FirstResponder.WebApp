using FirstResponder.ApplicationCore.Common.Exceptions;
using FirstResponder.ApplicationCore.Incidents.Commands;
using FirstResponder.ApplicationCore.Incidents.DTOs;
using FirstResponder.ApplicationCore.Incidents.Queries;
using FirstResponder.Web.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FirstResponder.Web.Controllers;

[Authorize(Policy = "IsEmployee")]
[Route("[controller]")]
public class IncidentsController : Controller
{
    private readonly IMediator _mediator;

    public IncidentsController(IMediator mediator)
    {
        _mediator = mediator;
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
    [Route("filtered-table-items")]
    public async Task<IEnumerable<IncidentItemDTO>> FilteredTableItems([FromQuery] IncidentItemFiltersDTO filtersDto, int pageNumber = 0, int pageSize = 0)
    {
        var items = await _mediator.Send(new GetIncidentItemsQuery { PageNumber = pageNumber, PageSize = pageSize, Filters = filtersDto });
        return items;
    }
}