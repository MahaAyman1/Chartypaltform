
using Chartypaltform.Data;
using Chartypaltform.Models;
using Chartypaltform.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

public class SuccessStoryController : Controller
{
    private readonly ApplicationDbContext _context;

    public SuccessStoryController(ApplicationDbContext context)
    {
        _context = context;
    }

   

    [HttpGet]
    public IActionResult CreateSuccessStory()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var completedCampaigns = _context.Campaigns
            .Where(c => c.UserId == userId && c.Status == CampaignStatus.Completed)
            .ToList();

        var viewModel = new CreateStoryViewModel
        {
            CompletedCampaigns = completedCampaigns ?? new List<Campaign>() 
        };

        if (!viewModel.CompletedCampaigns.Any())
        {
            ViewBag.Message = "No completed campaigns found for this user.";
        }

        return View(viewModel);
    }


    [HttpPost]
    public IActionResult CreateSuccessStory(CreateStoryViewModel model)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var campaign = _context.Campaigns.FirstOrDefault(c => c.CampaignId == model.SelectedCampaignId);
        if (campaign == null || campaign.UserId != userId || campaign.Status != CampaignStatus.Completed)
        {
            return BadRequest("Invalid campaign or permissions.");
        }

        List<string> imagePaths = new List<string>();
        if (model.ImageFiles != null && model.ImageFiles.Count > 0)
        {
            var imagesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/images");

            if (!Directory.Exists(imagesDirectory))
            {
                Directory.CreateDirectory(imagesDirectory);
            }

            foreach (var file in model.ImageFiles)
            {
                var fileName = Path.GetFileName(file.FileName);
                var filePath = Path.Combine(imagesDirectory, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                imagePaths.Add("/uploads/images/" + fileName); 
            }
        }

        string pdfPath = null;
        if (model.PdfFile != null)
        {
            var pdfDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/pdfs");

            if (!Directory.Exists(pdfDirectory))
            {
                Directory.CreateDirectory(pdfDirectory);
            }

            var pdfFileName = Path.GetFileName(model.PdfFile.FileName);
            var pdfFilePath = Path.Combine(pdfDirectory, pdfFileName);

            using (var stream = new FileStream(pdfFilePath, FileMode.Create))
            {
                model.PdfFile.CopyTo(stream);
            }

            pdfPath = "/uploads/pdfs/" + pdfFileName; 
        }

        var successStory = new SuccessCampaign
        {
            CampaignId = model.SelectedCampaignId,
            Campaign = campaign,
            impact = model.Impact,
            title = model.Title,
            ImagePaths = imagePaths,
            PdfPath = pdfPath,
            DateCreated = DateTime.Now
        };

        _context.successCampaigns.Add(successStory);
        _context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Index()
    {
        var successCampaigns = _context.successCampaigns
            .Include(sc => sc.Campaign)
            .ToList();
        return View(successCampaigns);
    }

    public async Task<IActionResult> Details(int id)
    {
        var eventItem = await _context.successCampaigns
            .Include(sc => sc.Campaign)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (eventItem == null)
        {
            return NotFound();
        }

        return View(eventItem);
    }

}
