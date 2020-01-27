using RetreatSchedule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RetreatSchedule.Helper
{
    public interface IEmailHelper
    {
        string GetCashPaymentMail(Booking booking);

        string GetCashPaymentSuccessfulMail(Booking booking);

        Task SendCashBookingSuccessfulEmailAsync(int bookingId);

        Task SendBookingDeletedEmailAsync(Booking booking);

        Task SendPaymentFailedEmailAsync(int bookingId);
    }
}
