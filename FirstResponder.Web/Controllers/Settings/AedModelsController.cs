using FirstResponder.ApplicationCore.Aeds.Commands;
using FirstResponder.ApplicationCore.Aeds.Queries;
using FirstResponder.ApplicationCore.Exceptions;
using FirstResponder.Web.Extensions;
using FirstResponder.Web.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<IActionResult> Index()
    {
        var models = await _mediator.Send(new GetAllModelsQuery());
        return View("../Settings/AedModels/Index", models);
    }
    
    [HttpPost]
    [Route("")]
    public async Task<IActionResult> Create(AedModelFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var models = await _mediator.Send(new GetAllModelsQuery());
            return View("../Settings/AedModels/Index", models);
        }

        try
        {
           await _mediator.Send(new CreateModelCommand(model.Name));
        }
        catch (EntityValidationException exception)
        {
            this.MapErrorsToErrorMessages(exception);
            return RedirectToAction(nameof(Index));
        }
        
        this.DisplaySuccessMessage("Model " + model.Name + " bol úspešne vytvorený!");
        return RedirectToAction(nameof(Index));
    }
    
    [HttpPost]
    [Route("{modelId}")]
    public async Task<IActionResult> Update(Guid modelId, AedModelFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var models = await _mediator.Send(new GetAllModelsQuery());
            return View("../Settings/AedModels/Index", models);
        }

        try
        {
            await _mediator.Send(new UpdateModelCommand(modelId, model.Name));
        }
        catch (EntityValidationException exception)
        {
            this.MapErrorsToErrorMessages(exception);
            return RedirectToAction(nameof(Index));
        }
        
        this.DisplaySuccessMessage("Názov modelu bol úspešne zmenený!");
        return RedirectToAction(nameof(Index));
    }
    
    [HttpPost]
    [Route("{modelId}/[action]")]
    public async Task<IActionResult> Delete(Guid modelId)
    {
        await _mediator.Send(new DeleteModelCommand(modelId));
        this.DisplaySuccessMessage("Model bol úspešne odstránený!");
        return RedirectToAction(nameof(Index));
    }
    
}