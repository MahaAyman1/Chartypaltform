using Chartypaltform.Models;
using Chartypaltform.Service;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Chartypaltform.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CampaignService _campaignService;

        public HomeController(ILogger<HomeController> logger , CampaignService campaignService)
        {
            _logger = logger;
            _campaignService = campaignService; 
        }

        public async Task<IActionResult> Index()
        {
            int completedCampaignCount = await _campaignService.GetCompletedCampaignCountAsync();
            ViewBag.CompletedCount = completedCampaignCount;
            return View();
        }

       
        public ActionResult Causes() {

            return View();

        }
        public ActionResult About()
        {

            return View();

        }

        public ActionResult Achievement()
        {

            return View();

        }
        public ActionResult Event()
        {

            return View();

        }

        public ActionResult Contact() { 

                return View();  
        }
       

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
