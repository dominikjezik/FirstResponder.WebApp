using FirstResponder.ApplicationCore.Exceptions;
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

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [Route("")]
    public async Task<IActionResult> Index()
    {
        var users = await _mediator.Send(new GetAllUsersQuery());
        return View(users);
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
            // TODO: Zmeniť fixné heslo
            var user = await _mediator.Send(new CreateUserCommand(model, "Password123!"));
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

        var model = user.ToUserFormDTO();

        return View(model);
    }
    
    [HttpPost]
    [Route("{userId}")]
    public async Task<IActionResult> Edit(string userId, UserFormDTO model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        
        try
        {
            var user = await _mediator.Send(new UpdateUserCommand(model));
            return RedirectToAction(nameof(Edit), "Users", new { userId = user.Id });
        }
        catch (EntityValidationException exception)
        {
            this.MapErrorsToModelState(exception);
            return View(model);
        }
    }

    [Route("[action]")]
    public IActionResult Map()
    {
        return View();
    }
    
}
