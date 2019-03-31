using Microsoft.EntityFrameworkCore;
using RetreatSchedule.Data;
using RetreatSchedule.Models;
using RetreatSchedule.Poco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RetreatSchedule.Helper
{
    public class DataTableHelper
    {
        private readonly RetreatDBContext _context;

        public DataTableHelper(RetreatDBContext context)
        {
            _context = context;
        }

        public async Task<PaginatedList<Booking>> GetBookingsAsync(DataTablesPageRequest pageRequest)
        {
            var bookingsQuery = _context.Bookings
                .Include(b => b.Activity)
                .Include(b => b.User)
                .Where(b => b.Id > 0);

            var page = (pageRequest.DisplayLength == 0) ? 1 : pageRequest.DisplayStart / pageRequest.DisplayLength + 1;
            return await PaginatedList<Booking>.CreateAsync(bookingsQuery.AsNoTracking(), page, pageRequest.DisplayLength);
        }
    }
}
