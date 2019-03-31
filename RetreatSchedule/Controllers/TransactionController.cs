using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RetreatSchedule.Areas.Identity.Services;
using RetreatSchedule.Config;
using RetreatSchedule.Data;
using RetreatSchedule.Helper;
using RetreatSchedule.Models;
using RetreatSchedule.Models.Enum;
using RetreatSchedule.Poco;
using RetreatSchedule.Poco.Paystack;
using RetreatSchedule.Services;
using RetreatSchedule.Util;
using RetreatSchedule.ViewModel;

namespace RetreatSchedule.Controllers
{
    public class TransactionController : Controller
    {
        private readonly RetreatDBContext _context;
        private readonly PaystackConfig _config;
        private readonly IRetreatEmailSender _emailSender;
        private readonly IEmailHelper _emailHelper;

        public TransactionController(RetreatDBContext context, IOptions<PaystackConfig> paystackConfig, IRetreatEmailSender emailSender, IEmailHelper emailHelper)
        {
            _context = context;
            _config = paystackConfig?.Value;
            _emailSender = emailSender;
            _emailHelper = emailHelper;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult FetchDetails([FromBody] EmailRequest emailRequest)
        {
            if (!string.IsNullOrWhiteSpace(emailRequest?.Email))
            {
                var user = _context.Users.SingleOrDefault(u => u.Email == emailRequest.Email.Trim());
                return Json(user);
            }
            return Json(null);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RaisePayment([FromBody] PaymentRequest request)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.SingleOrDefault(u => request.Email.Trim().Equals(u.Email, StringComparison.OrdinalIgnoreCase));
                if (user == null)
                {
                    user = new User { Email = request.Email.Trim().ToLower() };
                    SetUserDetails(request, user);
                    _context.Users.Add(user);
                }
                else
                {
                    SetUserDetails(request, user);
                    _context.Users.Update(user);
                }
                _context.SaveChanges();

                var bookingExists = _context.Bookings.Any(x => x.ActivityID == request.ActivityID && x.UserID == user.Id &&
                        (x.PaymentStatus == PaymentStatus.Successful || (x.PaymentType == PaymentType.Cash && x.PaymentStatus != PaymentStatus.Failed)));
                if (bookingExists)
                {
                    return Json(new Response { Code = "XX", Description = "You have already booked for this activity before." });
                }

                var activity = _context.Activities
                    .Include(x => x.ActivityType)
                    .SingleOrDefault(a => a.Id == request.ActivityID);

                if (activity == null || !activity.IsActive)
                    return Json(new Response { Code = "XX", Description = "Activity not found" });

                var bookingsCount = _context.Bookings.Count(a => a.ActivityID == request.ActivityID &&
                    (a.PaymentStatus == PaymentStatus.Successful || (a.PaymentType == PaymentType.Cash && a.PaymentStatus != PaymentStatus.Failed)));
                if (bookingsCount >= activity.Capacity)
                    return Json(new Response { Code = "XX", Description = "No space left, you can try again later." });

                var totalAmount = activity.Amount * request.NoOfSpaces;
                var booking = CreateBooking(request, user, activity, totalAmount);
                _context.Bookings.Add(booking);
                _context.SaveChanges();

                if (request.PaymentType == PaymentType.Cash)
                    SendCashBookingEmail(request, booking);
                if (request.PaymentType == PaymentType.Online)
                {
                    totalAmount = decimal.Parse(((totalAmount + 150) / (decimal)0.985).ToString("0"));
                }

                return Json(new Response
                {
                    Code = "00",
                    Description = "Successful",
                    Amount = (totalAmount * 100),
                    Email = user.Email,
                    Ref = booking.TransactionRef,
                    PaymentType = booking.PaymentType
                });
            }
            return Json(null);
        }

        private static Booking CreateBooking(PaymentRequest request, User user, Activity activity, decimal totalAmount)
        {
            return new Booking
            {
                ActivityID = request.ActivityID,
                NoOfSpaces = request.NoOfSpaces,
                Amount = totalAmount,
                PaymentStatus = PaymentStatus.Pending,
                UserID = user.Id,
                PaymentType = request.PaymentType,
                TransactionRef = RefGenerator.GenerateRef(),
                User = user,
                Activity = activity
            };
        }

        private static void SetUserDetails(PaymentRequest request, User user)
        {
            user.PhoneNumber = request.PhoneNo;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.AgeRange = request.AgeRange;
            user.ReferringCentreID = request.ReferringCentreID;
        }

        private void SendCashBookingEmail(PaymentRequest request, Booking booking)
        {
            try
            {
                var message = _emailHelper.GetCashPaymentMail(booking);
                _emailSender.SendEmailAsync(request.Email, $"{booking?.Activity?.ActivityType?.Name} Booking Pending", message, true);
            }
            catch (Exception e)
            { }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Donate([FromBody] DonationRequest request)
        {
            if (ModelState.IsValid)
            {
                if (request.Amount > 20000)
                    return Json(new Response { Code = "XX", Description = "You cannot donate above 20,000 naira. Please, enter a lesser amount." });

                if (string.IsNullOrWhiteSpace(request.Email))
                    request.Email = "bookings@retreatschedule.com";

                var donation = new Donation {
                    Name = request.Name,
                    Email = request.Email,
                    Phone = request.PhoneNumber,
                    Amount = request.Amount,
                    PaymentStatus = PaymentStatus.Pending,
                    TransactionRef = RefGenerator.GenerateRef()
                };
                _context.Donations.Add(donation);
                _context.SaveChanges();

                return Json(new Response
                {
                    Code = "00",
                    Description = "Successful",
                    Amount = (donation.Amount * 100),
                    Email = donation.Email,
                    Ref = donation.TransactionRef,
                    PaymentType = PaymentType.Online
                });
            }

            return Json(null);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Verify([FromBody] VerificationRequest request)
        {
            if (ModelState.IsValid)
            {
                var auth = $"Bearer {_config.SecretKey}";
                var response = await RestClient.GetJsonAsync<PaystackVerificationResponse>(auth, _config.VerifyUrl, request.TransactionRef);
                if (response == null)
                    return Json(null);

                var booking = _context.Bookings.SingleOrDefault(x => x.TransactionRef == request.TransactionRef);
                if (booking != null)
                {
                    var resp = new Response();
                    if (response.Data != null && "success".Equals(response.Data.Status))
                    {
                        booking.PaymentStatus = PaymentStatus.Successful;
                        resp.Code = "00";
                        resp.Description = "Your payment has been confirmed successful!";
                    }
                    else
                    {
                        booking.PaymentStatus = PaymentStatus.Failed;
                        resp.Code = "XX";
                        resp.Description = "Your payment failed. Please try again.";
                    }
                    booking.DateUpdated = DateTime.UtcNow.AddHours(1);
                    _context.Bookings.Update(booking);
                    await _context.SaveChangesAsync();
                    await _emailHelper.SendCashBookingSuccessfulEmailAsync(booking.Id);
                    return Json(resp);
                }
                else
                {
                    var donation = _context.Donations.SingleOrDefault(x => x.TransactionRef == request.TransactionRef);
                    if (donation != null)
                    {
                        var resp = new Response();
                        if (response.Data != null && "success".Equals(response.Data.Status))
                        {
                            donation.PaymentStatus = PaymentStatus.Successful;
                            resp.Code = "00";
                            resp.Description = "Your donation has been confirmed successful. Thank you!";
                        }
                        else
                        {
                            donation.PaymentStatus = PaymentStatus.Failed;
                            resp.Code = "XX";
                            resp.Description = "Your payment failed. Please try again.";
                        }
                        donation.DateUpdated = DateTime.UtcNow.AddHours(1);
                        _context.Donations.Update(donation);
                        await _context.SaveChangesAsync();
                        return Json(resp);
                    }
                }                 
            }
            return Json(null);
        }

    }
}