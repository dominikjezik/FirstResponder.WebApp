using FirstResponder.ApplicationCore.Abstractions;
using FirstResponder.Web.Filters;
using FirstResponder.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FirstResponder.Web.Controllers;

public class AccountController : Controller
{
    private readonly IAuthService _authService;

    public AccountController(IAuthService authService)
    {
        _authService = authService;
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
    
}