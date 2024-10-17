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

        // GET: Volunteering/Create
        [Authorize(Roles = "Donor")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Volunteering/Create
        [Authorize(Roles = "Donor")]
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(CreateVolunteeringViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Get the user ID from claims
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var volunteeringOpportunity = new Volunteering
                {
                    AvailableFrom = model.AvailableFrom,
                    AvailableTo = model.AvailableTo,
                    UserId = userId, // Use UserId instead of DonorId
                    TaskSelections = model.SelectedTasks.Select(task => new VolunteeringTaskSelection
                    {
                        TaskDescription = task
                    }).ToList()
                };

                _context.Volunteerings.Add(volunteeringOpportunity);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home"); // Redirect after creation
            }

            return View(model); // Return the view with the model if validation fails
        }

        public async Task<IActionResult> VolunteersList()
        {
            // Retrieve the data from the database
            var volunteerList = await _context.Volunteerings
                .Include(v => v.User) // Include the user details
                .Include(v => v.TaskSelections) // Include the selected tasks
                .ToListAsync(); // Load data into memory first


            // Process the data in memory
            var result = volunteerList.Select(v => new VolunteerListViewModel
            {
                UserName = v.User.UserName,
                // Assuming UserName is in ApplicationUser
                AvailableFrom = v.AvailableFrom,
                AvailableTo = v.AvailableTo,
                Age = (v.User is Donor donor) ? donor.Age : 0,
                Gender = (v.User is Donor donor1) ? donor1.Gender : null,
                Address = v.User.Address,
                SelectedTasks = v.TaskSelections.Select(ts => ts.TaskDescription.ToString()).ToList()
            }).ToList();

            return View(result); // Pass the processed data to the view
        }


        public async Task<IActionResult> VolunteersList1(string searchTerm, int? minAge, int? maxAge, string selectedGender)
        {
            // Retrieve the data from the database
            var volunteerQuery = _context.Volunteerings
                .Include(v => v.User) // Include user details
                .Include(v => v.TaskSelections) // Include selected tasks
                .AsQueryable(); // Start with IQueryable for efficient filtering

            // Apply filtering based on search terms (if provided)
            if (!string.IsNullOrEmpty(searchTerm))
            {
                volunteerQuery = volunteerQuery.Where(v =>
                    v.User.Address.Contains(searchTerm)); // Adjust based on your actual User properties
            }

            // Apply filtering based on age range (if provided)
            if (minAge.HasValue || maxAge.HasValue)
            {
                volunteerQuery = volunteerQuery.Where(v =>
                    v.User != null && // Check if User is not null
                    v.User.GetType() == typeof(Donor) && // Ensure it's a Donor
                    ((Donor)v.User).Age >= (minAge ?? 0) && // Cast and apply min age filter
                    ((Donor)v.User).Age <= (maxAge ?? int.MaxValue)); // Cast and apply max age filter
            }

            // Apply filtering based on selected gender (if provided)
            if (!string.IsNullOrEmpty(selectedGender))
            {
                volunteerQuery = volunteerQuery.Where(v =>
                    v.User != null && // Ensure User is not null
                    v.User.GetType() == typeof(Donor) && // Ensure it's a Donor
                    ((Donor)v.User).Gender.ToLower() == selectedGender.ToLower()); // Use ToLower() for case-insensitive comparison
            }

            // Execute the query and retrieve the list of volunteers
            var volunteerList = await volunteerQuery.ToListAsync();

            // Project to view model
            var result = volunteerList.Select(v => new VolunteerListViewModel
            {
                UserName = v.User.UserName,
                AvailableFrom = v.AvailableFrom,
                AvailableTo = v.AvailableTo,
                Age = (v.User is Donor donor) ? donor.Age : 0,
                Gender = (v.User is Donor donor1) ? donor1.Gender : null,
                Address = v.User.Address,
                SelectedTasks = v.TaskSelections.Select(ts => ts.TaskDescription.ToString()).ToList()
            }).ToList();

            ViewData["MinAge"] = minAge;
            ViewData["MaxAge"] = maxAge;
            ViewData["SelectedGender"] = selectedGender; // Store selected gender for the view
            return View("VolunteersList", result);
        }

    }
} 