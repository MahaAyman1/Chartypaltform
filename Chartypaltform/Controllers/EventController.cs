using Chartypaltform.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Chartypaltform.Data;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Chartypaltform.Controllers
{
    [Authorize]
    public class EventController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public EventController(UserManager<ApplicationUser> userManager, ApplicationDbContext context , IWebHostEnvironment hostEnvironment)
        {
            _userManager = userManager;
            _context = context;
            _hostEnvironment = hostEnvironment;

        }
        //If You Have Time Edit It To make The Donor Join To Multi Event 
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Event model)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                ModelState.AddModelError("", "User not found.");
                return View(model);
            }

            if (currentUser is CharityOrganization organization)
            {
                model.OrganizationId = organization.Id; 

                if (ModelState.IsValid)
                {
                    var uploadResult = await HandleFileUpload(model.formFile); 
                    if (uploadResult != null)
                    {
                        model.ImgUrl = uploadResult; 
                    }

                    model.DateCreated = DateTime.Now;

                    // Log the model before saving
                    System.Diagnostics.Debug.WriteLine($"Creating event: Title={model.Title}, OrganizationId={model.OrganizationId}");

                    // Save the event in the database
                    _context.Events.Add(model);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                ModelState.AddModelError("", "You must be a registered Charity Organization to create an event.");
            }

            return View(model); 
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Join(int eventId)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user is Donor donor)
            {
                var eventModel = await _context.Events
                    .Include(e => e.Attendees)
                    .FirstOrDefaultAsync(e => e.Id == eventId);

                if (eventModel == null || eventModel.Attendees.Count >= eventModel.MaxParticipants)
                {
                    return BadRequest("Event is full or not found.");
                }

                if (!eventModel.Attendees.Any(a => a.Id == donor.Id)) // Ensure donor is not already attending
                {
                    eventModel.Attendees.Add(donor);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index"); // Redirect to event list after joining
                }

                return BadRequest("You have already joined this event.");
            }

            return Unauthorized();
        }

        public async Task<IActionResult> Index()
        {
            await RemovePastEvents();

            var events = await _context.Events.ToListAsync();
            return View(events);
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var eventItem = await _context.Events
                .Include(e => e.Attendees) 
                .FirstOrDefaultAsync(e => e.Id == id);

            if (eventItem == null)
            {
                return NotFound();
            }

            return View(eventItem);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Leave(int eventId)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user is Donor donor)
            {
                var eventModel = await _context.Events
                    .Include(e => e.Attendees)
                    .FirstOrDefaultAsync(e => e.Id == eventId);

                if (eventModel == null)
                {
                    return NotFound("Event not found.");
                }

                // Find the donor in the event's attendees
                var attendee = eventModel.Attendees.FirstOrDefault(a => a.Id == donor.Id);
                if (attendee != null)
                {
                    eventModel.Attendees.Remove(attendee); // Remove the donor from this event
                    await _context.SaveChangesAsync(); // Save changes to the database

                    return RedirectToAction("Index"); // Redirect after leaving
                }

                return BadRequest("You are not attending this event.");
            }

            return Unauthorized();
        }
        private async Task RemovePastEvents()
        {
            var pastEvents = await _context.Events
                .Where(e => e.EventDate < DateTime.Now) 
                .ToListAsync();

            if (pastEvents.Any())
            {
                _context.Events.RemoveRange(pastEvents);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IActionResult> Attendees(int eventId)
        {
            var eventModel = await _context.Events
                .Include(e => e.Attendees)
                .FirstOrDefaultAsync(e => e.Id == eventId);

            if (eventModel == null)
            {
                return NotFound();
            }

            return View(eventModel.Attendees.ToList());
        }
        private async Task<string> HandleFileUpload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return "/uploads/" + uniqueFileName;
        }


    }
}
