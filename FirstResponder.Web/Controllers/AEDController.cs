using Microsoft.AspNetCore.Mvc;

namespace FirstResponder.Web.Controllers
{
    public class AEDController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Details(string aedId)
        {
            return View();
        }
        
        public IActionResult Map()
        {
            return View();
        }
    }
}