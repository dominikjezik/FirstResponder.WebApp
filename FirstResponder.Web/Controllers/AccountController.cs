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
    private readonly IMediator _mediator;
    private readonly IAuthService _authService;
    private readonly IMailService _mailService;

    public AccountController(IMediator mediator, IAuthService authService, IMailService mailService)
    {
        _mediator = mediator;
        _authService = authService;
        _mailService = mailService;
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
        var user = await _mediator.Send(new GetUserProfileByIdQuery(userId));

        if (user == null)
        {
            return NotFound();
        }

        return View(user);
    }
    
    [HttpPost]
    [Authorize]
    [Route("/account/profile")]
    public async Task<IActionResult> Profile(UserProfileDTO model)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _mediator.Send(new GetUserByIdQuery(userId));
        
        if (user == null)
        {
            return NotFound();
        }
        
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        
        try
        {
            await _mediator.Send(new UpdateUserCommand(user.UserId, model));
            
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
    
    # region Forgot Password
    
    [AllowOnlyAnonymous]
    [Route("/forgot-password")]
    public IActionResult ForgotPassword()
    {
        return View();
    }
    
    [HttpPost]
    [AllowOnlyAnonymous]
    [Route("/forgot-password")]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        
        var user = await _mediator.Send(new GetUserByEmailQuery(model.Email));
        
        if (user == null)
        {
            // If user is not found, it will not reveal that the user does not exist
            this.DisplaySuccessMessage("Na Váš email bol odoslaný odkaz na obnovenie hesla!");
            return RedirectToAction(nameof(ForgotPassword));
        }
        
        try
        {
            var token = await _authService.GeneratePasswordResetTokenAsync(model.Email);
            var resetPasswordUrl = Url.Action(nameof(ResetPassword), "Account", new { token, email = model.Email },
                HttpContext.Request.Scheme);

            var mailBody = $"Svoje heslo si môžete obnoviť na <a href='{resetPasswordUrl}'>tomto odkaze</a>.";
            var isSuccessful = _mailService.SendMail(model.Email, user.FullName, "Obnovenie hesla", mailBody);
            
            if (!isSuccessful)
            {
                this.DisplayErrorMessage("Nepodarilo sa odoslať email na obnovenie hesla");
                return View(model);
            }
        }
        catch (EntityNotFoundException exception)
        {
        }
        
        this.DisplaySuccessMessage("Na Váš email bol odoslaný odkaz na obnovenie hesla!");
        return RedirectToAction(nameof(ForgotPassword));
    }
    
    #endregion
    
    #region Reset Password
    
    [AllowOnlyAnonymous]
    [Route("/reset-password")]
    public IActionResult ResetPassword(string? token = null, string? email = null)
    {
        if (string.IsNullOrEmpty(token))
        {
            return NotFound();
        }
        
        return View(new ResetPasswordViewModel { Token = token, Email = email});
    }

    [HttpPost]
    [AllowOnlyAnonymous]
    [Route("/reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            // If user is not found, it will not reveal that the user does not exist
            await _authService.ResetPasswordAsync(model.Email, model.Token, model.NewPassword);
            
            return RedirectToAction(nameof(ResetPasswordConfirmation));
        }
        catch (EntityValidationException exception)
        {
            this.MapErrorsToModelState(exception);
            return View(model);
        }
    }
    
    [AllowAnonymous]
    [Route("/reset-password-confirmation")]
    public IActionResult ResetPasswordConfirmation()
    {
        return View();
    }

    #endregion
}