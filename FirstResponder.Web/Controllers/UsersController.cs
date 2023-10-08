using Microsoft.AspNetCore.Mvc;

namespace FirstResponder.Web.Controllers;

public class UsersController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Details(string userId)
    {
        return View();
    }
    
    public IActionResult Create()
    {
        return View();
    }

    public IActionResult Map()
    {
        return View();
    }
    
}
