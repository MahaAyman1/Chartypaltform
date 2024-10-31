using Chartypaltform.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Chartypaltform.ViewComponents
{
    public class LatestEventViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _db;

        public LatestEventViewComponent(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var events = await _db.Events.Include(e => e.DonorEvents)
                .ThenInclude(de => de.Donor).OrderByDescending(e => e.EventDate).Take(3)
        .ToListAsync();
            return View(events);
        }
    }
}
