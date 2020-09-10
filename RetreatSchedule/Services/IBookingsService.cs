using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RetreatSchedule.Models;
using RetreatSchedule.Models.Enum;

namespace RetreatSchedule.Services
{
    public interface IBookingsService
    {
        bool Exists(int id);

        Task<Booking> FindById(int? id); 

        Task<Booking> FindByIdLoadActivityType(int? id);

        IQueryable<Booking> FetchAllBookingsWithCentre();

        IQueryable<Booking> fetchBookings(int? ActivityID, MemberType? MemberTypeID, string Attended);
    }
}
