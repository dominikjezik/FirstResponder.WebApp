using FirstResponder.ApplicationCore.Common.Exceptions;
using FirstResponder.ApplicationCore.Courses.Commands;
using FirstResponder.ApplicationCore.Courses.DTOs;
using FirstResponder.ApplicationCore.Courses.Queries;
using FirstResponder.ApplicationCore.Entities.CourseAggregate;
using FirstResponder.Web.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FirstResponder.Web.Controllers;

[Authorize(Policy = "IsEmployee")]
[Route("[controller]")]
public class CoursesController : Controller
{
    private readonly IMediator _mediator;

    public CoursesController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [Route("")]
    public async Task<IActionResult> Index()
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
    public async Task<IActionResult> Create(CourseFormDTO model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var course = await _mediator.Send(new CreateCourseCommand(model));
            this.DisplaySuccessMessage("Školenie bolo úspešne vytvorené.");
            return RedirectToAction(nameof(Edit), new { courseId = course.Id });
        }
        catch (EntityValidationException exception)
        {
            this.MapErrorsToModelState(exception);
            return View(model);
        }
    }
    
    [Route("{courseId}")]
    public async Task<IActionResult> Edit(Guid courseId)
    {
        var course = await _mediator.Send(new GetCourseByIdQuery(courseId));

        if (course == null)
        {
            return NotFound();
        }
        
        return View(course);
    }
    
    [HttpPost]
    [Route("{courseId}")]
    public async Task<IActionResult> Edit(Guid courseId, CourseDTO model)
    {
        var course = await _mediator.Send(new GetCourseByIdQuery(courseId));

        if (course == null)
        {
            return NotFound();
        }
        
        if (!ModelState.IsValid)
        {
            // Ensure that the original data remains filled in
            course.CourseForm = model.CourseForm;
            return View(course);
        }
        
        try
        {
            await _mediator.Send(new UpdateCourseCommand(course.CourseId, model.CourseForm));
            this.DisplaySuccessMessage("Školenie bolo úspešne aktualizované!");
            return RedirectToAction(nameof(Edit), new { courseId = course.CourseId });
        }
        catch (EntityNotFoundException)
        {
            return NotFound();
        }
        catch (EntityValidationException exception)
        {
            this.MapErrorsToModelState(exception);
            // Ensure that the original data remains filled in
            course.CourseForm = model.CourseForm;
            return View(course);
        }
    }
    
    [HttpPost]
    [Route("{courseId}/[action]")]
    public async Task<IActionResult> Delete(Guid courseId)
    {
        try
        {
            await _mediator.Send(new DeleteCourseCommand(courseId));
            this.DisplaySuccessMessage("Školenie bolo úspešne vymazané!");
            return RedirectToAction(nameof(Index));
        }
        catch (EntityNotFoundException)
        {
            return NotFound();
        }
    }
    
    [HttpGet]
    [Route("[action]")]
    public async Task<IEnumerable<CourseType>> Types()
    {
        // TODO
        /*
        var types = await _mediator.Send(new GetAllCourseTypesQuery());
        return types;
        */
        return new List<CourseType>();
    }
    
    [HttpGet]
    [Route("filtered-table-items")]
    public async Task<IEnumerable<Course>> FilteredTableItems(int pageNumber, [FromQuery] CourseFiltersDTO filtersDTO)
    {
        return await _mediator.Send(new GetCoursesQuery() { PageNumber = pageNumber, Filters = filtersDTO });
    }
}