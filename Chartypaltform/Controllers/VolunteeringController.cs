
using Chartypaltform.Data;
using Chartypaltform.Models;
using Chartypaltform.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Chartypaltform.Controllers
{
    [Authorize]
    public class VolunteeringController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VolunteeringController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "Donor")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Donor")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateVolunteeringViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var volunteeringOpportunity = new Volunteering
                {
                    AvailableFrom = model.AvailableFrom,
                    AvailableTo = model.AvailableTo,
                    UserId = userId,
                    TaskSelections = model.SelectedTasks.Select(task => new VolunteeringTaskSelection
                    {
                        TaskDescription = task
                    }).ToList(),
                };

                _context.Volunteerings.Add(volunteeringOpportunity);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index" , "Home");
            }

            return View(model);
        }

        public async Task<IActionResult> VolunteersList()
        {
            var volunteerList = await _context.Volunteerings
                .Include(v => v.User) 
                .Include(v => v.TaskSelections) 
                .ToListAsync(); 

          
            var result = volunteerList.Select(v => new VolunteerListViewModel
            {
                UserName = v.User.UserName,
                AvailableFrom = v.AvailableFrom,
                AvailableTo = v.AvailableTo,
                Age = (v.User is Donor donor) ? donor.Age : 0,
                Gender = (v.User is Donor donor1) ? donor1.Gender : null,
                Address = v.User.Address,
                FullName = (v.User is Donor donor2) ? donor2.FullName : "",
                phone = v.User.PhoneNumber,
                SelectedTasks = v.TaskSelections.Select(ts => ts.TaskDescription.ToString()).ToList() 
            }).ToList();

            return View(result); 
        }


        public IActionResult MyVolunteeringOpportunities()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 

            var opportunities = _context.Volunteerings
                .Where(opportunity => opportunity.UserId == userId)
                .Include(v => v.TaskSelections) 
                .ToList();

            var viewModel = opportunities.Select(opportunity => new VolunteerListViewModel
            {
                Id = opportunity.Id,
                AvailableFrom = opportunity.AvailableFrom,
                AvailableTo = opportunity.AvailableTo,
                SelectedTasks = opportunity.TaskSelections.Select(ts => ts.TaskDescription.ToString()).ToList() 
            }).ToList();

            return View(viewModel); 
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var volunteeringOpportunity = await _context.Volunteerings
                .Include(v => v.TaskSelections)
                .FirstOrDefaultAsync(v => v.Id == id);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (volunteeringOpportunity == null || volunteeringOpportunity.UserId != userId)
            {
                return Forbid();
            }

            var model = new CreateVolunteeringViewModel
            {
                Id = volunteeringOpportunity.Id, 
                AvailableFrom = volunteeringOpportunity.AvailableFrom,
                AvailableTo = volunteeringOpportunity.AvailableTo,
                SelectedTasks = volunteeringOpportunity.TaskSelections.Select(ts => ts.TaskDescription).ToList()
            };

            return View(model); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateVolunteeringViewModel model)
        {
            if (ModelState.IsValid)
            {
                var volunteeringOpportunity = await _context.Volunteerings
                    .Include(v => v.TaskSelections)
                    .FirstOrDefaultAsync(v => v.Id == id);

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (volunteeringOpportunity == null || volunteeringOpportunity.UserId != userId)
                {
                    return Forbid(); 
                }

                volunteeringOpportunity.AvailableFrom = model.AvailableFrom;
                volunteeringOpportunity.AvailableTo = model.AvailableTo;

                volunteeringOpportunity.TaskSelections.Clear();
                volunteeringOpportunity.TaskSelections = model.SelectedTasks.Select(task => new VolunteeringTaskSelection
                {
                    TaskDescription = task 
                }).ToList();

                await _context.SaveChangesAsync();
                return RedirectToAction("MyVolunteeringOpportunities"); 
            }

            return View(model); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> Delete(int id)
        {
            var volunteering = await _context.Volunteerings.FindAsync(id);

            if (volunteering == null)
            {
                return NotFound();
            }

            _context.Volunteerings.Remove(volunteering);

            await _context.SaveChangesAsync();

              return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> VolunteersList1(string searchTerm, int? minAge, int? maxAge, string selectedGender)
        {
            var volunteerQuery = _context.Volunteerings
                .Include(v => v.User) 
                .Include(v => v.TaskSelections) 
                .AsQueryable(); 

            if (!string.IsNullOrEmpty(searchTerm))
            {
                volunteerQuery = volunteerQuery.Where(v =>
                    v.User.Address.Contains(searchTerm)); 
            }

            if (minAge.HasValue || maxAge.HasValue)
            {
                volunteerQuery = volunteerQuery.Where(v =>
                    v.User != null && 
                    v.User.GetType() == typeof(Donor) && 
                    ((Donor)v.User).Age >= (minAge ?? 0) &&
                    ((Donor)v.User).Age <= (maxAge ?? int.MaxValue));
            }

            if (!string.IsNullOrEmpty(selectedGender))
            {
                volunteerQuery = volunteerQuery.Where(v =>
                    v.User != null && 
                    v.User.GetType() == typeof(Donor) && 
                    ((Donor)v.User).Gender.ToLower() == selectedGender.ToLower()); 
            }

            var volunteerList = await volunteerQuery.ToListAsync();


            var result = volunteerList.Select(v => new VolunteerListViewModel
            {
                UserName = v.User.UserName,
                AvailableFrom = v.AvailableFrom,
                AvailableTo = v.AvailableTo,
                Age = (v.User is Donor donor) ? donor.Age : 0,
                Gender = (v.User is Donor donor1) ? donor1.Gender : null,
                Address = v.User.Address,
                FullName = (v.User is Donor donor2) ? donor2.FullName : "",
                phone = v.User.PhoneNumber, 
                SelectedTasks = v.TaskSelections.Select(ts => ts.TaskDescription.ToString()).ToList()
            }).ToList();

            ViewData["MinAge"] = minAge;
            ViewData["MaxAge"] = maxAge;
            ViewData["SelectedGender"] = selectedGender; 
            return View("VolunteersList", result);

        }
    }
}
