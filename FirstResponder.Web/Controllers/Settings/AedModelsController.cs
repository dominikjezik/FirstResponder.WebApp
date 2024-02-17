using FirstResponder.ApplicationCore.Aeds.Commands;
using FirstResponder.ApplicationCore.Aeds.Queries;
using FirstResponder.ApplicationCore.Common.Exceptions;
using FirstResponder.ApplicationCore.Entities.AedAggregate;
using FirstResponder.ApplicationCore.Exceptions;
using FirstResponder.Web.Extensions;
using FirstResponder.Web.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FirstResponder.Web.Controllers.Settings;

[Authorize(Policy = "IsEmployee")]
[Route("settings/aed/models")]
public class AedModelsController : Controller
{
    private readonly IMediator _mediator;

    public AedModelsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [Route("")]
    public async Task<IActionResult> Index(string? manufacturer = null)
    {
        try
        {
            IEnumerable<Model> models = new List<Model>();
            
            if (manufacturer != null)
            {
                models = await _mediator.Send(new GetAllModelsQuery { ManufacturerId = manufacturer });
            }
            
            ViewBag.ManufacturerId = manufacturer;
            await LoadManufacturersToViewBag(manufacturer);

            return View("../Settings/AedModels/Index", models);
        }
        catch (EntityNotFoundException exception)
        {
            this.DisplayErrorMessage(exception.Message);
            return RedirectToAction(nameof(Index));
        }
    }
    
    [HttpPost]
    [Route("")]
    public async Task<IActionResult> Create(AedModelFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var models = await _mediator.Send(new GetAllModelsQuery());
            await LoadManufacturersToViewBag();
            return View("../Settings/AedModels/Index", models);
        }

        try
        {
            await _mediator.Send(new CreateModelCommand(model.Name, model.ManufacturerId));
        }
        catch (EntityNotFoundException exception)
        {
            this.DisplayErrorMessage(exception.Message);
            return RedirectToAction(nameof(Index), new { manufacturer = model.ManufacturerId });
        }
        catch (EntityValidationException exception)
        {
            this.MapErrorsToErrorMessages(exception);
            return RedirectToAction(nameof(Index), new { manufacturer = model.ManufacturerId });
        }
        
        this.DisplaySuccessMessage("Model " + model.Name + " bol úspešne vytvorený!");
        return RedirectToAction(nameof(Index), new { manufacturer = model.ManufacturerId });
    }
    
    [HttpPost]
    [Route("{modelId}")]
    public async Task<IActionResult> Update(Guid modelId, AedModelFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var models = await _mediator.Send(new GetAllModelsQuery());
            await LoadManufacturersToViewBag();
            return View("../Settings/AedModels/Index", models);
        }

        try
        {
            await _mediator.Send(new UpdateModelCommand(modelId, model.Name));
        }
        catch (EntityNotFoundException exception)
        {
            this.DisplayErrorMessage(exception.Message);
            return RedirectToAction(nameof(Index), new { manufacturer = model.ManufacturerId });
        }
        catch (EntityValidationException exception)
        {
            this.MapErrorsToErrorMessages(exception);
            return RedirectToAction(nameof(Index), new { manufacturer = model.ManufacturerId });
        }
        
        this.DisplaySuccessMessage("Názov modelu bol úspešne zmenený!");
        return RedirectToAction(nameof(Index), new { manufacturer = model.ManufacturerId });
    }
    
    [HttpPost]
    [Route("{modelId}/[action]")]
    public async Task<IActionResult> Delete(Guid modelId, string? manufacturerId = null)
    {
        await _mediator.Send(new DeleteModelCommand(modelId));
        this.DisplaySuccessMessage("Model bol úspešne odstránený!");
        return RedirectToAction(nameof(Index), new { manufacturer = manufacturerId });
    }
    
    #region Helpers
    
    private async Task LoadManufacturersToViewBag(string selectedManufacturer = null)
    {
        ViewBag.Manufacturers = (await _mediator.Send(new GetAllManufacturersQuery()))
            .Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString(), Selected = m.Id.ToString() == selectedManufacturer});
    }
    
    #endregion
    
}