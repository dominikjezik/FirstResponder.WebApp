using Microsoft.AspNetCore.Mvc;

namespace FirstResponder.Web.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        if (!User.Identity.IsAuthenticated)
        {
            return RedirectToAction(nameof(AccountController.Login), "Account");
        }
        
        return View();
    }
}