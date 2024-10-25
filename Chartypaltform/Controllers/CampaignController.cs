
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
          /*  var model = new CampaignViewModel
            {
                Categories = _context.categories.Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.Name
                }).ToList()
            };*/

            return View(/*model*/);
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

                // Create campaign
                var campaign = new Campaign
                {
                    CampaignImg = model.CampaignImg, // Now populated with the file URL
                    CampaignName = model.CampaignName,
                    CampaignDes = model.CampaignDes,
                    GoalAmount = model.GoalAmount,
                    CurrentAmountRaised = 0,
                    Status = CampaignStatus.Pending,
                    CreatedAt = DateTime.Now,
                    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier), // Get the ID of the logged-in user
                   // Categories = new List<Category>()
                };

                // Add selected categories to the campaign
             /*   foreach (var categoryId in model.SelectedCategoryIds)
                {
                    var category = await _context.categories.FindAsync(categoryId);
                    if (category != null)
                    {
                        campaign.Categories.Add(category);
                    }
                }*/

                // Save to the database
                _context.Campaigns.Add(campaign);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // If model state is invalid, reload the categories and return the view
        /*    model.Categories = _context.categories.Select(c => new SelectListItem
            {
                Value = c.CategoryId.ToString(),
                Text = c.Name
            }).ToList();*/
            return View(/*model*/);
        }

        public async Task<IActionResult> Index()
        {
            var campaigns = await _context.Campaigns
               // .Include(c => c.Categories)
                .Include(c => c.User) // Include the CharityOrganization details
                .ToListAsync();

            return View(campaigns);
        }
    }
}

