using FirstResponder.ApplicationCore.Aeds.Commands;
using FirstResponder.ApplicationCore.Aeds.Queries;
using FirstResponder.ApplicationCore.Common.Exceptions;
using FirstResponder.Web.Extensions;
using FirstResponder.Web.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FirstResponder.Web.Controllers.Settings;

[Authorize(Policy = "IsEmployee")]
[Route("settings/aed/manufacturers")]
public class AedManufacturersController : Controller
{
    private readonly IMediator _mediator;

    public AedManufacturersController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [Route("")]
    public async Task<IActionResult> Index()
    {
        var manufacturers = await _mediator.Send(new GetAllManufacturersQuery());
        return View("../Settings/AedManufacturers/Index", manufacturers);
    }

    [HttpPost]
    [Route("")]
    public async Task<IActionResult> Create(AedManufacturerFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var manufacturers = await _mediator.Send(new GetAllManufacturersQuery());
            return View("../Settings/AedManufacturers/Index", manufacturers);
        }

        try
        {
            await _mediator.Send(new CreateManufacturerCommand(model.Name));
        }
        catch (EntityValidationException exception)
        {
            this.MapErrorsToErrorMessages(exception);
            return RedirectToAction(nameof(Index));
        }
        
        this.DisplaySuccessMessage("Výrobca " + model.Name + " bol úspešne vytvorený!");
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [Route("{manufacturerId}")]
    public async Task<IActionResult> Update(Guid manufacturerId, AedManufacturerFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var manufacturers = await _mediator.Send(new GetAllManufacturersQuery());
            return View("../Settings/AedManufacturers/Index", manufacturers);
        }

        try
        {
            await _mediator.Send(new UpdateManufacturerCommand(manufacturerId, model.Name));
        }
        catch (EntityValidationException exception)
        {
            this.MapErrorsToErrorMessages(exception);
            return RedirectToAction(nameof(Index));
        }
        
        this.DisplaySuccessMessage("Názov výrobcu bol úspešne zmenený!");
        return RedirectToAction(nameof(Index));
    }
    
    [HttpPost]
    [Route("{manufacturerId}/[action]")]
    public async Task<IActionResult> Delete(Guid manufacturerId)
    {
        await _mediator.Send(new DeleteManufacturerCommand(manufacturerId));
        this.DisplaySuccessMessage("Výrobca bol úspešne odstránený!");
        return RedirectToAction(nameof(Index));
    }
}