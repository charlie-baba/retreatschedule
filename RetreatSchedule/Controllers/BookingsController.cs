using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RetreatSchedule.Data;
using RetreatSchedule.Helper;
using RetreatSchedule.Models;
using RetreatSchedule.Models.Enum;
using RetreatSchedule.Poco;

namespace RetreatSchedule.Controllers
{
    [Authorize]
    public class BookingsController : Controller
    {
        private readonly RetreatDBContext _context;
        private readonly IEmailHelper _emailHelper;
        private readonly DataTableHelper _dataTableHelper;

        public BookingsController(RetreatDBContext context,  
            IEmailHelper emailHelper)
        {
            _context = context;
            _emailHelper = emailHelper;
            _dataTableHelper = new DataTableHelper(context);
        }

        // GET: Bookings
        public async Task<IActionResult> Index(int? ActivityID, MemberType? MemberTypeID, string Attended)
        {
            Console.WriteLine(Attended);
            var bookingsQuery = _context.Bookings
                .Include(b => b.Activity)
                .Include(b => b.User)
                .Include("User.ReferringCentre")
                .Where(b => b.Id > 0);

            if (ActivityID != null)
                bookingsQuery = bookingsQuery.Where(b => b.ActivityID == ActivityID);
            if (MemberTypeID != null)
                bookingsQuery = bookingsQuery.Where(b => b.User.MemberType == MemberTypeID);
            if (!string.IsNullOrWhiteSpace(Attended))
                bookingsQuery = bookingsQuery.Where(b => b.IsAttended == true);

            ViewData["MemberType"] = new SelectList(Enum.GetValues(typeof(MemberType)).Cast<MemberType>().Select(m => new SelectListItem
            {
                Text = m.ToString()
            }).ToList(), "Text", "Text", MemberTypeID);
            ViewData["ActivityID"] = new SelectList(_context.Activities.Where(a => a.IsActive), "Id", "Title", ActivityID);
            ViewData["Attended"] = Attended;
            return View(await bookingsQuery.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> FetchBookings(DataTablesPageRequest pageRequest)
        {
            var bookingsQuery = _context.Bookings
                .Include(b => b.Activity)
                .Include(b => b.User)
                .Include("User.ReferringCentre")
                .Where(b => b.Id > 0);

            var page = (pageRequest.DisplayLength == 0) ? 1 : pageRequest.DisplayStart / pageRequest.DisplayLength + 1;
            var bookings = await PaginatedList<Booking>.CreateAsync(bookingsQuery.AsNoTracking(), page, pageRequest.DisplayLength);
            return Json(new DataTablesPageResponse<Booking>
            {
                Draw = 1,
                RecordsTotal = bookings.Count(),
                RecordsFiltered = bookings.Count(),
                Data = bookings.ToList()
            });
        }

        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Activity)
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // GET: Bookings/Create
        //public IActionResult Create()
        //{
        //    ViewData["ActivityID"] = new SelectList(_context.Activities, "Id", "PictureUrl");
        //    ViewData["UserID"] = new SelectList(_context.Users, "Id", "FirstName");
        //    return View();
        //}

        // POST: Bookings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("UserID,PaymentStatus,Amount,PaymentType,ActivityID,Id")] Booking booking)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(booking);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["ActivityID"] = new SelectList(_context.Activities, "Id", "PictureUrl", booking.ActivityID);
        //    ViewData["UserID"] = new SelectList(_context.Users, "Id", "FirstName", booking.UserID);
        //    return View(booking);
        //}

        // GET: Bookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings.Include(b => b.User).Include(b => b.Activity).FirstOrDefaultAsync(m => m.Id == id);
            if (booking == null)
            {
                return NotFound();
            }
            ViewData["ActivityID"] = new SelectList(_context.Activities.Where(a => a.IsActive), "Id", "Title", booking.ActivityID);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserID,TransactionRef,PaymentStatus,Amount,DateCreated,PaymentType,ActivityID,IsAttended,Id")] Booking booking)
        {
            if (id != booking.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();

                    // send email notification
                    if (booking.PaymentType == PaymentType.Cash && booking.PaymentStatus == PaymentStatus.Successful)
                    {
                        await _emailHelper.SendCashBookingSuccessfulEmailAsync(id);
                    }
                    else if (booking.PaymentStatus == PaymentStatus.Failed)
                    {
                        await _emailHelper.SendPaymentFailedEmailAsync(id);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.Id))
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
            ViewData["ActivityID"] = new SelectList(_context.Activities, "Id", "Title", booking.ActivityID);
            return View(booking);
        }

        // GET: Bookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .Include(b => b.Activity)
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Bookings
                            .Include(b => b.User)
                            .Include(b => b.Activity)
                            .Include("Activity.ActivityType")
                            .FirstOrDefaultAsync(m => m.Id == id);
            if (booking.PaymentStatus == PaymentStatus.Successful)
            {
                return NotFound();
            }

            var temp = booking;
            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
            await _emailHelper.SendBookingDeletedEmailAsync(temp);
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return _context.Bookings.Any(e => e.Id == id);
        }

    }
}
