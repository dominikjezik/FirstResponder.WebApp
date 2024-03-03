using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Common.DTOs;
using FirstResponder.ApplicationCore.Common.Exceptions;
using FirstResponder.ApplicationCore.Courses.Commands;
using FirstResponder.ApplicationCore.Courses.DTOs;
using FirstResponder.ApplicationCore.Courses.Queries;
using FirstResponder.ApplicationCore.Entities.CourseAggregate;
using FirstResponder.ApplicationCore.Entities.UserAggregate;
using FirstResponder.ApplicationCore.Groups.Queries;
using FirstResponder.Web.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FirstResponder.Web.Controllers;

[Authorize(Policy = "IsEmployee")]
[Route("[controller]")]
public class CoursesController : Controller
{
    private readonly IMediator _mediator;
    private readonly IMailService _mailService;

    public CoursesController(IMediator mediator, IMailService mailService)
    {
        _mediator = mediator;
        _mailService = mailService;
    }
    
    [Route("")]
    public async Task<IActionResult> Index()
    {
        return View();
    }
    
    [Route("[action]")]
    public async Task<IActionResult> Create()
    {
        await LoadCourseTypesToViewBag();
        return View();
    }
    
    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> Create(CourseFormDTO model)
    {
        if (!ModelState.IsValid)
        {
            await LoadCourseTypesToViewBag();
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
            await LoadCourseTypesToViewBag();
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
        
        await LoadCourseTypesToViewBag();
        
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
            await LoadCourseTypesToViewBag();
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
            await LoadCourseTypesToViewBag();
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
    
    [HttpPost]
    [Route("{courseId}/[action]")]
    public async Task<IActionResult> SendEmails(Guid courseId)
    {
        try
        {
            var course = await _mediator.Send(new GetCourseByIdQuery(courseId));
            
            if (course == null)
            {
                return NotFound();
            }
            
            var emailsAndNames = course.Participants
                .Select(m => Tuple.Create(m.Email, m.FullName))
                .ToArray();
            
            var mailBody = "<h1>Pozvánka na školenie</h1>" +
                "<p>Boli ste pozvaný na školenie " + course.CourseForm.Name + ".</p>" +
                "<p>Typ školenia: " + (course.CourseTypeName ?? "-") + "</p>" +
                "<p>Školenie sa bude konať " + course.CourseForm.StartDate.ToString("dd.MM.yyyy HH:mm") + " - " + course.CourseForm.EndDate.ToString("dd.MM.yyyy HH:mm") + ".</p>" +
                "<p>Adresa: " + course.CourseForm.Location + "</p>" +
                "<p>Školiteľ: " + course.CourseForm.Trainer + "</p>" +
                "<p>Popis: " + course.CourseForm.Description + "</p>";
            
            var isSuccessful = _mailService.SendMailToMultipleRecipients(emailsAndNames, "Pozvánka na školenie", mailBody);
            
            if (!isSuccessful)
            {
                this.DisplayErrorMessage("Nepodarilo sa odoslať email na obnovenie hesla");
                return RedirectToAction(nameof(Edit), new { courseId });
            }
            
            this.DisplaySuccessMessage("Pozvánky boli úspešne odoslané!");
            return RedirectToAction(nameof(Edit), new { courseId });
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
        var types = await _mediator.Send(new GetAllCourseTypesQuery());
        return types;
    }
    
    [HttpGet]
    [Route("filtered-table-items")]
    public async Task<IEnumerable<Course>> FilteredTableItems(int pageNumber, [FromQuery] CourseFiltersDTO filtersDTO)
    {
        return await _mediator.Send(new GetCoursesQuery() { PageNumber = pageNumber, Filters = filtersDTO });
    }
    
    [HttpGet]
    [Route("{courseId}/users")]
    public async Task<IEnumerable<UserWithAssociationInfoDTO>> Users(Guid courseId, string query = "")
    {
        var users = await _mediator.Send(new GetUsersWithCourseInfoQuery(courseId, query));
        return users;
    }
    
    [HttpPost]
    [IgnoreAntiforgeryToken]
    [Route("{entityId}/users")]
    public async Task<IActionResult> ChangeUsers([FromBody] UsersAssociationChangeDTO model)
    {
        await _mediator.Send(new ChangeUsersInCourseCommand(model));
        return Ok();
    }
    
    [HttpGet]
    [Route("groups")]
    public async Task<IEnumerable<Group>> Groups()
    {
        return await _mediator.Send(new GetAllGroupsQuery());
    }
    
    #region Helpers
    
    private async Task LoadCourseTypesToViewBag()
    {
        ViewBag.CourseTypes = (await _mediator.Send(new GetAllCourseTypesQuery()))
            .Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() });
    }
    
    #endregion
}