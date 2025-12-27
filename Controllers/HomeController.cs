using Microsoft.AspNetCore.Mvc;

namespace Kursach_CorpHubPortal.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Title = $"{ ViewBag.Brand ?? "Корпаротивный портал"}: главная страница";
            return View();
        }
    }
}
