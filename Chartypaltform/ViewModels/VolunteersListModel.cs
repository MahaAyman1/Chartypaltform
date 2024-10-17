using Chartypaltform.Data;
using Chartypaltform.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
namespace Chartypaltform.ViewModels
{
    public class VolunteersListModel : PageModel
    {
        private readonly ApplicationDbContext _context; // Use your DbContext

        public VolunteersListModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<VolunteerListViewModel> VolunteerList { get; set; } = new List<VolunteerListViewModel>();

        // Filter parameters

        [BindProperty(SupportsGet = true)]
        public string Gender { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? MinAge { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? MaxAge { get; set; }

        [BindProperty(SupportsGet = true)]
        public VolunteeringTask? Task { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Address { get; set; }

        public async Task OnGetAsync()
        {
            // Your VolunteersList logic goes here
            var volunteerQuery = _context.Volunteerings
                .Include(v => v.User) // Include the User (which can be a Donor)
                .Include(v => v.TaskSelections) // Include task selections
                .Where(v => v.User is Donor)
                .AsQueryable();

            if (!string.IsNullOrEmpty(Gender))
            {
                volunteerQuery = volunteerQuery.Where(v => ((Donor)v.User).Gender == Gender);
            }

            if (MinAge.HasValue)
            {
                volunteerQuery = volunteerQuery.Where(v => ((Donor)v.User).Age >= MinAge.Value);
            }

            if (MaxAge.HasValue)
            {
                volunteerQuery = volunteerQuery.Where(v => ((Donor)v.User).Age <= MaxAge.Value);
            }

            if (Task.HasValue)
            {
                volunteerQuery = volunteerQuery.Where(v => v.TaskSelections.Any(ts => ts.TaskDescription == Task.Value));
            }

            if (!string.IsNullOrEmpty(Address))
            {
                volunteerQuery = volunteerQuery.Where(v => v.User.Address.Contains(Address));
            }

            var volunteerList = await volunteerQuery.ToListAsync();

            VolunteerList = volunteerList.Select(v => new VolunteerListViewModel
            {
                UserName = v.User.UserName,
                AvailableFrom = v.AvailableFrom,
                AvailableTo = v.AvailableTo,
                Age = ((Donor)v.User).Age,
                Gender = ((Donor)v.User).Gender,
                Address = v.User.Address,
                SelectedTasks = v.TaskSelections.Select(ts => ts.TaskDescription.ToString()).ToList()
            }).ToList();
        }
    }
}
