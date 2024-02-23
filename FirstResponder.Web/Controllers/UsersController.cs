using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Common.Exceptions;
using FirstResponder.ApplicationCore.Users.Commands;
using FirstResponder.ApplicationCore.Users.DTOs;
using FirstResponder.ApplicationCore.Users.Queries;
using FirstResponder.Web.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FirstResponder.Web.Controllers;

[Authorize(Policy = "IsEmployee")]
[Route("[controller]")]
public class UsersController : Controller
{
    private readonly IMediator _mediator;
    private readonly IAuthService _authService;
    private readonly IMailService _mailService;

    public UsersController(IMediator mediator, IAuthService authService, IMailService mailService)
    {
        _mediator = mediator;
        _authService = authService;
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
        return View();
    }
    
    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> Create(UserFormDTO model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        
        try
        {
            var user = await _mediator.Send(new CreateUserCommand(model));
            this.DisplaySuccessMessage("Používateľ bol úspešne vytvorený!");
            return RedirectToAction(nameof(Edit), "Users", new { userId = user.Id });
        }
        catch (EntityValidationException exception)
        {
            this.MapErrorsToModelState(exception);
            return View(model);
        }
    }
    
    [Route("{userId}")]
    public async Task<IActionResult> Edit(string userId)
    {
        var user = await _mediator.Send(new GetUserByIdQuery(userId));

        if (user == null)
        {
            return NotFound();
        }

        return View(user);
    }
    
    [HttpPost]
    [Route("{userId}")]
    public async Task<IActionResult> Edit(string userId, UserDTO model)
    {
        var user = await _mediator.Send(new GetUserByIdQuery(userId));
        
        if (user == null)
        {
            return NotFound();
        }
        
        if (!ModelState.IsValid)
        {
            user.UserForm = model.UserForm;
            return View(model);
        }
        
        try
        {
            await _mediator.Send(new UpdateUserCommand(user.UserId, model.UserForm));
            this.DisplaySuccessMessage("Používateľ bol úspešne aktualizovaný!");
            return RedirectToAction(nameof(Edit), "Users", new { userId = user.UserId });
        }
        catch (EntityValidationException exception)
        {
            this.MapErrorsToModelState(exception);
            return View(model);
        }
    }
    
    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> SendPasswordResetLink(string userId)
    {
        var user = await _mediator.Send(new GetUserProfileByIdQuery(userId));
        
        if (user == null)
        {
            return NotFound();
        }

        try
        {
            var token = await _authService.GeneratePasswordResetTokenAsync(user.Email);
            var resetPasswordUrl = Url.Action("ResetPassword", "Account", new { token, email = user.Email  }, HttpContext.Request.Scheme);
        
            var mailBody = $"Svoje nové heslo si môžete nastaviť na <a href='{resetPasswordUrl}'>tomto odkaze</a>.";
            var isSuccessful = _mailService.SendMail(user.Email, user.FullName, "Obnovenie hesla", mailBody);
        
            if (!isSuccessful)
            {
                this.DisplayErrorMessage("Nepodarilo sa odoslať email na obnovenie hesla");
                return RedirectToAction(nameof(Edit), "Users", new { userId });
            }
        }
        catch (EntityNotFoundException)
        {
            return NotFound();
        }
        
        this.DisplaySuccessMessage("Odkaz na resetovanie hesla bol úspešne odoslaný!");
        return RedirectToAction(nameof(Edit), "Users", new { userId });
    }

    [Route("[action]")]
    public IActionResult Map()
    {
        return View();
    }
    
    [Route("search")]
    public async Task<IEnumerable<UserSearchResultDTO>> Search(string query)
    {
        var searchResults = await _mediator.Send(new GetUsersBySearchQuery(query));
        return searchResults;
    }
    
    [Route("filtered-table-items")]
    public async Task<IEnumerable<UserItemDTO>> FilteredTableItems(int pageNumber, [FromQuery] UserItemFiltersDTO filtersDto)
    {
        var items = await _mediator.Send(new GetUserItems() { PageNumber = pageNumber, Filters = filtersDto });
        return items;
    }
}