using Microsoft.AspNetCore.Mvc;

namespace FirstResponder.Web.Controllers;

[Route("[controller]")]
public class ErrorController : Controller
{
    [Route("")]
    public IActionResult Error()
    {
        ViewBag.TitleMessage = "500 Nastala chyba!";
        ViewBag.Message = "Internal server error.";
        return View("StatusCode");
    }
    
    [Route("{statusCode}")]
    public IActionResult StatusCode(int statusCode)
    {
        ViewBag.StatusCode = statusCode;
        
        if (statusCode == 404)
        {
            ViewBag.TitleMessage = "404 Stránka sa nenašla!";
            ViewBag.Message = "Nepodarilo sa nájsť žiadanú stránku.";
            return View();
        }
        return View();
    }
}