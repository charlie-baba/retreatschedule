using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RetreatSchedule.Data;
using RetreatSchedule.Models;
using RetreatSchedule.Util;

namespace RetreatSchedule.Controllers
{
    [Authorize]
    public class ActivitiesController : Controller
    {
        private readonly RetreatDBContext _context;

        public ActivitiesController(RetreatDBContext context)
        {
            _context = context;
        }

        // GET: Activities
        public async Task<IActionResult> Index()
        {
            var retreatDBContext = _context.Activities.Include(a => a.ActivityType).Include(a => a.Location);
            return View(await retreatDBContext.ToListAsync());
        }

        // GET: Activities/Create
        public IActionResult Create()
        {
            ViewData["ActivityTypeID"] = new SelectList(_context.ActivityTypes, "Id", "Name");
            ViewData["LocationID"] = new SelectList(_context.Locations, "Id", "Name");
            return View();
        }

        // POST: Activities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,PictureUrl,Capacity,StartDate,EndDate,Amount,HouseKeepingInfo,IsActive,LocationID,ActivityTypeID,Id")] Activity activity)
        {
            var currentUser = HttpContext.Session.Get<User>(Constants.SessionKeyUser);
            if (currentUser?.IsViewOnly ?? true)
                return Forbid();

            if (ModelState.IsValid)
            {
                _context.Add(activity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ActivityTypeID"] = new SelectList(_context.ActivityTypes, "Id", "Name", activity.ActivityTypeID);
            ViewData["LocationID"] = new SelectList(_context.Locations, "Id", "Name", activity.LocationID);
            return View(activity);
        }

        // GET: Activities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _context.Activities.FindAsync(id);
            if (activity == null)
            {
                return NotFound();
            }
            ViewData["ActivityTypeID"] = new SelectList(_context.ActivityTypes, "Id", "Name", activity.ActivityTypeID);
            ViewData["LocationID"] = new SelectList(_context.Locations, "Id", "Name", activity.LocationID);
            return View(activity);
        }

        // POST: Activities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Title,Description,PictureUrl,Capacity,StartDate,EndDate,Amount,HouseKeepingInfo,IsActive,LocationID,ActivityTypeID,Id")] Activity activity)
        {
            var currentUser = HttpContext.Session.Get<User>(Constants.SessionKeyUser);
            if (currentUser == null || id != activity.Id)
                return NotFound();

            if (currentUser.IsViewOnly)
                return Forbid();
            
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(activity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActivityExists(activity.Id))
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
            ViewData["ActivityTypeID"] = new SelectList(_context.ActivityTypes, "Id", "Name", activity.ActivityTypeID);
            ViewData["LocationID"] = new SelectList(_context.Locations, "Id", "Name", activity.LocationID);
            return View(activity);
        }

        // GET: Activities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _context.Activities
                .Include(a => a.ActivityType)
                .Include(a => a.Location)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (activity == null)
            {
                return NotFound();
            }

            return View(activity);
        }

        // POST: Activities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var activity = await _context.Activities.FindAsync(id);
            _context.Activities.Remove(activity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActivityExists(int id)
        {
            return _context.Activities.Any(e => e.Id == id);
        }
    }
}
