using Chartypaltform.Data;
using Chartypaltform.Models;
using Chartypaltform.Service;
using iText.Html2pdf;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;

namespace Chartypaltform.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CampaignService _campaignService;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger , CampaignService campaignService , ApplicationDbContext context)
        {
            _logger = logger;
            _campaignService = campaignService; 
            _context = context; 
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


        [HttpPost]
        public FileResult Export()
        {
            // Fetching the first 10 campaigns.
            List<object> campaigns = (from campaign in this._context.Campaigns.Take(10)
                                      select new[] {
                                  campaign.CampaignId.ToString(),
                                  campaign.CampaignName,
                                  campaign.CampaignDes,
                                  campaign.GoalAmount.ToString(),
                                  campaign.CurrentAmountRaised.ToString(),
                                  campaign.Status.ToString(),
                                  campaign.CreatedAt.ToString("yyyy-MM-dd")
                              }).ToList<object>();

            // Building an HTML string.
            StringBuilder sb = new StringBuilder();

            // Table start.
            sb.Append("<table border='1' cellpadding='5' cellspacing='0' style='border: 1px solid #ccc;font-family: Arial; font-size: 10pt;'>");

            // Building the Header row.
            sb.Append("<tr>");
            sb.Append("<th style='background-color: #B8DBFD;border: 1px solid #ccc'>Campaign ID</th>");
            sb.Append("<th style='background-color: #B8DBFD;border: 1px solid #ccc'>Campaign Name</th>");
            sb.Append("<th style='background-color: #B8DBFD;border: 1px solid #ccc'>Description</th>");
            sb.Append("<th style='background-color: #B8DBFD;border: 1px solid #ccc'>Goal Amount</th>");
            sb.Append("<th style='background-color: #B8DBFD;border: 1px solid #ccc'>Current Amount Raised</th>");
            sb.Append("<th style='background-color: #B8DBFD;border: 1px solid #ccc'>Status</th>");
            sb.Append("<th style='background-color: #B8DBFD;border: 1px solid #ccc'>Created At</th>");
            sb.Append("</tr>");

            // Building the Data rows.
            foreach (string[] campaign in campaigns)
            {
                sb.Append("<tr>");
                foreach (string field in campaign)
                {
                    sb.Append("<td style='border: 1px solid #ccc'>");
                    sb.Append(field);
                    sb.Append("</td>");
                }
                sb.Append("</tr>");
            }

            // Table end.
            sb.Append("</table>");

            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(sb.ToString())))
            {
                using (MemoryStream byteArrayOutputStream = new MemoryStream())
                {
                    PdfWriter writer = new PdfWriter(byteArrayOutputStream);
                    PdfDocument pdfDocument = new PdfDocument(writer);
                    pdfDocument.SetDefaultPageSize(PageSize.A4);
                    HtmlConverter.ConvertToPdf(stream, pdfDocument);
                    pdfDocument.Close();
                    return File(byteArrayOutputStream.ToArray(), "application/pdf", "Campaigns.pdf");
                }
            }
        }

            [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
