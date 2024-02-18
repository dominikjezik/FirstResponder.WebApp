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
            // Zabezpeci, aby ostali zachovane povodne vyplnene data
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
            // Zabezpeci, aby ostali zachovane povodne vyplnene data
            incident.IncidentForm = model.IncidentForm;
            return View(model);
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
    
    [HttpGet]
    [Route("filtered-table-items")]
    public async Task<IEnumerable<IncidentItemDTO>> FilteredTableItems(int pageNumber, [FromQuery] IncidentItemFiltersDTO filtersDto)
    {
        var items = await _mediator.Send(new GetIncidentItemsQuery { PageNumber = pageNumber, Filters = filtersDto });
        return items;
    }
}