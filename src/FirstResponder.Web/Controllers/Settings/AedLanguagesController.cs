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
[Route("settings/aed/languages")]
public class AedLanguagesController : Controller
{
    private readonly IMediator _mediator;

    public AedLanguagesController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [Route("")]
    public async Task<IActionResult> Index()
    {
        var languages = await _mediator.Send(new GetAllLanguagesQuery());
        return View("../Settings/AedLanguages/Index", languages);
    }
    
    [HttpPost]
    [Route("")]
    public async Task<IActionResult> Create(AedLanguageFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var languages = await _mediator.Send(new GetAllLanguagesQuery());
            return View("../Settings/AedLanguages/Index", languages);
        }

        try
        {
            await _mediator.Send(new CreateLanguageCommand(model.Name));
        }
        catch (EntityValidationException exception)
        {
            this.MapErrorsToErrorMessages(exception);
            return RedirectToAction(nameof(Index));
        }
        
        this.DisplaySuccessMessage("Jazyk " + model.Name + " bol úspešne vytvorený!");
        return RedirectToAction(nameof(Index));
    }
    
    [HttpPost]
    [Route("{languageId}")]
    public async Task<IActionResult> Update(Guid languageId, AedLanguageFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var languages = await _mediator.Send(new GetAllLanguagesQuery());
            return View("../Settings/AedLanguages/Index", languages);
        }

        try
        {
            await _mediator.Send(new UpdateLanguageCommand(languageId, model.Name));
        }
        catch (EntityValidationException exception)
        {
            this.MapErrorsToErrorMessages(exception);
            return RedirectToAction(nameof(Index));
        }
        
        this.DisplaySuccessMessage("Názov jazyka bol úspešne zmenený!");
        return RedirectToAction(nameof(Index));
    }
    
    [HttpPost]
    [Route("{languageId}/[action]")]
    public async Task<IActionResult> Delete(Guid languageId)
    {
        await _mediator.Send(new DeleteLanguageCommand(languageId));
        this.DisplaySuccessMessage("Jazyk bol úspešne odstránený!");
        return RedirectToAction(nameof(Index));
    }
}