using Microsoft.AspNetCore.Mvc;

namespace Chartypaltform.Areas.Administrator.Controllers
{
    [Area("Administrator")]

    public class CpanelController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult RedirectToHome()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}