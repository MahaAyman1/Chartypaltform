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
                    .Include(e => e.DonorEvents)
                    .FirstOrDefaultAsync(e => e.Id == eventId);

                if (eventModel == null)
                {
                    return View("Errors", "Event not found.");
                }
                if (eventModel.DonorEvents.Count >= eventModel.MaxParticipants)
                {
                    return View("Errors", "Event is full.");
                }

                if (!eventModel.DonorEvents.Any(de => de.DonorId == donor.Id))
                {
                    var donorEvent = new DonorEvent
                    {
                        DonorId = donor.Id,
                        EventId = eventModel.Id
                    };

                    eventModel.DonorEvents.Add(donorEvent);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Event" , "Home"); 
                }

                return View("Errors", "You have already joined this event.");
            }

            return Unauthorized();
        }


     

        public async Task<IActionResult> Index()
        {
            await RemovePastEvents();

            var events = await _context.Events.ToListAsync();
            return View(events);
        }

       
        public async Task<IActionResult> Details(int id)
        {
            var eventItem = await _context.Events
                .Include(e => e.DonorEvents) 
                    .ThenInclude(de => de.Donor) 
                .FirstOrDefaultAsync(e => e.Id == id);

            if (eventItem == null)
            {
                return NotFound();
            }

            return View(eventItem);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Leave(int eventId)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user is Donor donor)
            {
                var donorEvent = await _context.DonorEvents
                    .FirstOrDefaultAsync(de => de.EventId == eventId && de.DonorId == donor.Id);

                if (donorEvent != null)
                {
                    _context.DonorEvents.Remove(donorEvent); 
                    await _context.SaveChangesAsync(); 

                    return RedirectToAction("Event", "Home"); 
                }

                return BadRequest("You are not attending this event.");
            }

            return Unauthorized();
        }

        public async Task<IActionResult> Attendees(int eventId)
        {
            var eventModel = await _context.Events
                .Include(e => e.DonorEvents)
                    .ThenInclude(de => de.Donor) 
                .FirstOrDefaultAsync(e => e.Id == eventId);

            if (eventModel == null)
            {
                return NotFound();
            }

            var attendees = eventModel.DonorEvents.Select(de => de.Donor).ToList(); 

            return View(attendees);
        }


    }
}
