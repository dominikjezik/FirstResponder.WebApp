using System.Security.Claims;
using FirstResponder.ApplicationCore.Common.Abstractions;
using FirstResponder.ApplicationCore.Common.Exceptions;
using FirstResponder.ApplicationCore.Users.Commands;
using FirstResponder.ApplicationCore.Users.DTOs;
using FirstResponder.ApplicationCore.Users.Queries;
using FirstResponder.Web.Extensions;
using FirstResponder.Web.Filters;
using FirstResponder.Web.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FirstResponder.Web.Controllers;

public class AccountController : Controller
{
    private readonly IAuthService _authService;
    private readonly IMediator _mediator;

    public AccountController(IAuthService authService, IMediator mediator)
    {
        _authService = authService;
        _mediator = mediator;
    }
    
    #region Login

    [AllowOnlyAnonymous]
    [Route("/[action]")]
    public IActionResult Login()
    {
        return View();
    }
    
    [HttpPost]
    [AllowOnlyAnonymous]
    [Route("/[action]")]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await _authService.PasswordSignInAsync(model.Email, model.Password, isPersistent: true);

        if (!result)
        {
            ModelState.AddModelError(string.Empty, "Nesprávne prihlasovacie údaje");
            return View(model);
        }

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return LocalRedirect(returnUrl);
        }

        return RedirectToAction(nameof(HomeController.Index), "Home");
    }

    #endregion

    #region Logout

    [HttpPost]
    [Route("/[action]")]
    public async Task<IActionResult> Logout()
    {
        await _authService.SignOutAsync();
        return RedirectToAction(nameof(HomeController.Index), "Home");
    }

    #endregion

    [Authorize]
    [Route("/account")]
    public IActionResult Index()
    {
        return View();
    }
    
    #region Update Profile

    [Authorize]
    [Route("/account/profile")]
    public async Task<IActionResult> Profile()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _mediator.Send(new GetUserByIdQuery(userId));

        if (user == null)
        {
            return NotFound();
        }

        return View(user);
    }
    
    [HttpPost]
    [Authorize]
    [Route("/account/profile")]
    public async Task<IActionResult> Profile(UserDTO model)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _mediator.Send(new GetUserByIdQuery(userId));
        
        if (user == null)
        {
            return NotFound();
        }
        
        // Type of user cannot be changed
        model.UserForm.UserType = user.UserForm.UserType;
        
        if (!ModelState.IsValid)
        {
            user.UserForm = model.UserForm;
            return View(model);
        }
        
        try
        {
            await _mediator.Send(new UpdateUserCommand(user.UserId, model.UserForm));
            
            // Update claims
            await _authService.RefreshSignInAsync(user.UserId);
            
            this.DisplaySuccessMessage("Profil bol úspešne aktualizovaný");
            return RedirectToAction(nameof(Profile));
        }
        catch (EntityValidationException exception)
        {
            this.MapErrorsToModelState(exception);
            return View(model);
        }
    }

    #endregion
    
    #region Change Password
    
    [Authorize]
    [Route("/account/change-password")]
    public IActionResult ChangePassword()
    {
        return View();
    }
    
    [HttpPost]
    [Authorize]
    [Route("/account/change-password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        try
        {
            await _authService.ChangePasswordAsync(userId, model.CurrentPassword, model.NewPassword, refreshSignIn: true);
        }
        catch (EntityValidationException exception)
        {
            this.MapErrorsToModelState(exception);
            return View(model);
        }
        
        this.DisplaySuccessMessage("Heslo bolo úspešne zmenené");
        return RedirectToAction(nameof(ChangePassword));
    }
    
    #endregion
}