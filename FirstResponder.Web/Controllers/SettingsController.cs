using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FirstResponder.Web.Controllers;

[Authorize(Policy = "IsEmployee")]
[Route("[controller]")]
public class SettingsController : Controller
{
    [Route("[action]")]
    public IActionResult Aed()
    {
        return View();
    }
}