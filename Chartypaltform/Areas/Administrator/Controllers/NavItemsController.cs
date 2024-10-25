using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Chartypaltform.Data;
using Chartypaltform.Models;

namespace Chartypaltform.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    public class NavItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NavItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Administrator/NavItems
        public async Task<IActionResult> Index()
        {
            return View(await _context.navItems.ToListAsync());
        }

        // GET: Administrator/NavItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var navItem = await _context.navItems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (navItem == null)
            {
                return NotFound();
            }

            return View(navItem);
        }

        // GET: Administrator/NavItems/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Administrator/NavItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MenuName,MenuUrl,Visibility")] NavItem navItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(navItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(navItem);
        }

        // GET: Administrator/NavItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var navItem = await _context.navItems.FindAsync(id);
            if (navItem == null)
            {
                return NotFound();
            }
            return View(navItem);
        }

        // POST: Administrator/NavItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MenuName,MenuUrl,Visibility")] NavItem navItem)
        {
            if (id != navItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(navItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NavItemExists(navItem.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(navItem);
        }

        // GET: Administrator/NavItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var navItem = await _context.navItems
                .FirstOrDefaultAsync(m => m.Id == id);
            if (navItem == null)
            {
                return NotFound();
            }

            return View(navItem);
        }

        // POST: Administrator/NavItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var navItem = await _context.navItems.FindAsync(id);
            if (navItem != null)
            {
                _context.navItems.Remove(navItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NavItemExists(int id)
        {
            return _context.navItems.Any(e => e.Id == id);
        }
    }
}
