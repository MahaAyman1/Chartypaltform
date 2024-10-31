using Chartypaltform.Data;
using iText.Commons.Actions.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chartypaltform.ViewComponents
{
    public class EventViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _db;

        public EventViewComponent(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var events = await _db.Events
                .Include(e => e.DonorEvents) 
                .ThenInclude(de => de.Donor) 
                .Where(e => e.EventDate >= DateTime.Now) 
                .ToListAsync();
            return View(events);
        }
    }
}
