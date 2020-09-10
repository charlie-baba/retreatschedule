using Microsoft.EntityFrameworkCore;
using RetreatSchedule.Data;
using RetreatSchedule.Models;
using RetreatSchedule.Models.Enum;
using System.Linq;
using System.Threading.Tasks;

namespace RetreatSchedule.Services.Impl
{
    public class BookingsService : IBookingsService
    {
        private readonly RetreatDBContext _context;
        public BookingsService(RetreatDBContext context)
        {
            _context = context;
        }

        public bool Exists(int id)
        {
            return _context.Bookings.Any(e => e.Id == id);
        }

        public IQueryable<Booking> FetchAllBookingsWithCentre()
        {
            return _context.Bookings
                .Include(b => b.Activity)
                .Include(b => b.User)
                .Include("User.ReferringCentre")
                .Where(b => b.Id > 0);
        }

        IQueryable<Booking> IBookingsService.fetchBookings(int? ActivityID, MemberType? MemberTypeID, string Attended)
        {
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

            return bookingsQuery;
        }

        Task<Booking> IBookingsService.FindById(int? id)
        {
            return _context.Bookings
                .Include(b => b.Activity)
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        Task<Booking> IBookingsService.FindByIdLoadActivityType(int? id)
        {
            return _context.Bookings
                    .Include(b => b.User)
                    .Include(b => b.Activity)
                    .Include("Activity.ActivityType")
                    .FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
