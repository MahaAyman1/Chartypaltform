
using Microsoft.AspNetCore.Mvc;
using Chartypaltform.Models;
using Chartypaltform.ViewModels;
using System.Threading.Tasks;
using Chartypaltform.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Chartypaltform.Service;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Collections.Generic;
using System.Linq;
namespace Chartypaltform.Controllers
{
    public class CampaignController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CampaignController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Create()
        {
         

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CampaignViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Handle file upload
                if (model.formFile != null)
                {
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                    var fileUrl = await FileUploadHelper.HandleFileUpload(model.formFile, uploadsFolder);

                    if (!string.IsNullOrEmpty(fileUrl))
                    {
                        model.CampaignImg = fileUrl; // Set the CampaignImg field after upload
                    }
                    else
                    {
                        ModelState.AddModelError("", "File upload failed.");
                        model.Categories = _context.categories.Select(c => new SelectListItem
                        {
                            Value = c.CategoryId.ToString(),
                            Text = c.Name
                        }).ToList();
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError("formFile", "Image is required.");
                    model.Categories = _context.categories.Select(c => new SelectListItem
                    {
                        Value = c.CategoryId.ToString(),
                        Text = c.Name
                    }).ToList();
                    return View(model);
                }

                var campaign = new Campaign
                {
                    CampaignImg = model.CampaignImg, 
                    CampaignName = model.CampaignName,
                    CampaignDes = model.CampaignDes,
                    GoalAmount = model.GoalAmount,
                    CurrentAmountRaised = 0,
                    Status = CampaignStatus.Pending,
                    CreatedAt = DateTime.Now,
                    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier), 
                };

              
                
                _context.Campaigns.Add(campaign);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        public async Task<IActionResult> Index()
        {
            var campaigns = await _context.Campaigns
                .Include(c => c.User) 
                .ToListAsync();

            return View(campaigns);
        }

        public async Task<IActionResult> Details(int id)
        {
            var C = await _context.Campaigns
                .FirstOrDefaultAsync(o => o.CampaignId == id);

            if (C == null)
            {
                return NotFound();
            }

            return View(C);
        }

     
    }
}

