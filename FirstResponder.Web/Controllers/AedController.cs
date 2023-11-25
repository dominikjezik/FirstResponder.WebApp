using FirstResponder.ApplicationCore.Aeds.Commands;
using FirstResponder.ApplicationCore.Aeds.DTOs;
using FirstResponder.ApplicationCore.Aeds.Queries;
using FirstResponder.ApplicationCore.Entities.AedAggregate;
using FirstResponder.ApplicationCore.Exceptions;
using FirstResponder.Web.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FirstResponder.Web.Controllers;

[Authorize(Policy = "IsEmployee")]
[Route("[controller]")]
public class AedController : Controller
{
    private readonly IMediator _mediator;

    public AedController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [Route("")]
    public async Task<IActionResult> Index()
    {
        await LoadManufacturersToViewBag();
        await LoadModelsToViewBag();
        
        var aeds = await _mediator.Send(new GetAllAedsQuery());
        return View(aeds);
    }

    [Route("[action]")]
    public async Task<IActionResult> Create()
    {
        await LoadOptionsForSelectionsToViewBag();
        return View();
    }

    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> Create(AedFormDTO model)
    {
        if (!ModelState.IsValid)
        {
            await LoadOptionsForSelectionsToViewBag();
            return View(model);
        }
        
        try
        {
            var aed = await _mediator.Send(new CreateAedCommand(model));
            return RedirectToAction(nameof(Edit), "Aed", new { aedId = aed.Id });
        }
        catch (EntityValidationException exception)
        {
            this.MapErrorsToModelState(exception);
            await LoadOptionsForSelectionsToViewBag();
            return View(model);
        }
    }

    [Route("{aedId}")]
    public async Task<IActionResult> Edit(string aedId)
    {
        var aed = await _mediator.Send(new GetAedByIdQuery(aedId));

        if (aed == null)
        {
            return NotFound();
        }

        var model = aed.ToAedFormDTO();
        
        return View(model);
    }

    [HttpPost]
    [Route("{aedId}")]
    public async Task<IActionResult> Edit(string aedId, AedFormDTO model)
    {
        var aed = await _mediator.Send(new GetAedByIdQuery(aedId));

        if (aed == null)
        {
            return NotFound();
        }
        
        if (!ModelState.IsValid)
        {
            model.CreatedAt = aed.CreatedAt;
            
            if (aed is PersonalAed personalAed)
            {
                model.Owner = personalAed.Owner;
            }
            
            await LoadOptionsForSelectionsToViewBag();
            return View(model);
        }

        try
        {
            var updatedAed = await _mediator.Send(new UpdateAedCommand(model));
            return RedirectToAction(nameof(Edit), "Aed", new { aedId = updatedAed.Id });
        }
        catch (EntityNotFoundException)
        {
            return NotFound();
        }
        catch (EntityValidationException exception)
        {
            this.MapErrorsToModelState(exception);
            
            model.CreatedAt = aed.CreatedAt;
            if (aed is PersonalAed personalAed)
            {
                model.Owner = personalAed.Owner;
            }

            await LoadOptionsForSelectionsToViewBag();
            return View(model);
        }
    }

    [HttpPost]
    [Route("{aedId}/[action]")]
    public async Task<IActionResult> Delete(string aedId)
    {
        try
        {
            await _mediator.Send(new DeleteAedCommand(aedId));
            return RedirectToAction(nameof(Index), "Aed");
        }
        catch (ArgumentException)
        {
            return NotFound();
        }
        catch (EntityNotFoundException)
        {
            return NotFound();
        }
    }

    [Route("[action]")]
    public IActionResult Map()
    {
        // TODO: Získanie a zobrazenie verejných AED na mape
        return View();
    }

    #region Helpers

    private async Task LoadOptionsForSelectionsToViewBag()
    {
        await LoadManufacturersToViewBag();
        await LoadModelsToViewBag();
        await LoadLanguagesToViewBag();
    }

    private async Task LoadManufacturersToViewBag()
    {
        ViewBag.Manufacturers = (await _mediator.Send(new GetAllManufacturersQuery()))
            .Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
    }
    
    private async Task LoadModelsToViewBag()
    {
        ViewBag.Models = (await _mediator.Send(new GetAllModelsQuery()))
            .Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
    }

    private async Task LoadLanguagesToViewBag()
    {
        ViewBag.Languages = (await _mediator.Send(new GetAllLanguagesQuery()))
            .Select(l => new SelectListItem { Text = l.Name, Value = l.Id.ToString() });
    }

    #endregion
    
}