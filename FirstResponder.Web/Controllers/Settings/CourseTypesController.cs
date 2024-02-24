using FirstResponder.ApplicationCore.Common.Exceptions;
using FirstResponder.ApplicationCore.Courses.Commands;
using FirstResponder.ApplicationCore.Courses.Queries;
using FirstResponder.Web.Extensions;
using FirstResponder.Web.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FirstResponder.Web.Controllers.Settings;

[Authorize(Policy = "IsEmployee")]
[Route("settings/users/course-types")]
public class CourseTypesController : Controller
{
    private readonly IMediator _mediator;
    
    public CourseTypesController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [Route("")]
    public async Task<IActionResult> Index()
    {
        var courseTypes = await _mediator.Send(new GetAllCourseTypesQuery());
        return View("../Settings/CourseTypes/Index", courseTypes);
    }
    
    [HttpPost]
    [Route("")]
    public async Task<IActionResult> Create(CourseTypeFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var courseTypes = await _mediator.Send(new GetAllCourseTypesQuery());
            return View("../Settings/CourseTypes/Index", courseTypes);
        }

        try
        {
            await _mediator.Send(new CreateCourseTypeCommand(model.Name));
        }
        catch (EntityValidationException exception)
        {
            this.MapErrorsToErrorMessages(exception);
            return RedirectToAction(nameof(Index));
        }
        
        this.DisplaySuccessMessage("Typ školenia " + model.Name + " bol úspešne vytvorený!");
        return RedirectToAction(nameof(Index));
    }
    
    [HttpPost]
    [Route("{courseTypeId}")]
    public async Task<IActionResult> Update(Guid courseTypeId, CourseTypeFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var courseTypes = await _mediator.Send(new GetAllCourseTypesQuery());
            return View("../Settings/CourseTypes/Index", courseTypes);
        }

        try
        {
            await _mediator.Send(new UpdateCourseTypeCommand(courseTypeId, model.Name));
        }
        catch (EntityValidationException exception)
        {
            this.MapErrorsToErrorMessages(exception);
            return RedirectToAction(nameof(Index));
        }
        
        this.DisplaySuccessMessage("Typ školenia bol úspešne zmenený!");
        return RedirectToAction(nameof(Index));
    }
    
    [HttpPost]
    [Route("{courseTypeId}/[action]")]
    public async Task<IActionResult> Delete(Guid courseTypeId)
    {
        await _mediator.Send(new DeleteCourseTypeCommand(courseTypeId));
        this.DisplaySuccessMessage("Typ školenia bol úspešne odstránený!");
        return RedirectToAction(nameof(Index));
    }
}