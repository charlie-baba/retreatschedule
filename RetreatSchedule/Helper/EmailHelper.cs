using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using RetreatSchedule.Areas.Identity.Services;
using RetreatSchedule.Data;
using RetreatSchedule.Models;
using static RetreatSchedule.Util.SettingConstants;

namespace RetreatSchedule.Helper
{
    public class EmailHelper : IEmailHelper
    {
        private string _templateDir;
        private readonly RetreatDBContext _context;
        private readonly IRetreatEmailSender _emailSender;

        public EmailHelper(IHostingEnvironment env, RetreatDBContext context, IRetreatEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
            _templateDir = Path.Combine(env.WebRootPath, $"templates\\email_templates\\");
        }

        public string GetCashPaymentMail(Booking booking) {
            var message = GetFileContent("Cash_Payment_Initiated.html");            
            message = message.Replace("{Name}", $"{booking.User?.FirstName} {booking.User?.LastName}");
            message = message.Replace("{TransRef}", booking.TransactionRef);
            message = message.Replace("{Date}", booking.DateCreated.ToString());
            message = message.Replace("{Title}", booking.Activity?.Title);
            message = message.Replace("{Amount}", string.Format("{0:#,##0}", booking.Amount));
            message = message.Replace("{Bank}", _context.Settings.FirstOrDefault(s => s.Name == BankName)?.Value);
            message = message.Replace("{AcctName}", _context.Settings.FirstOrDefault(s => s.Name == AccountName)?.Value);
            message = message.Replace("{AcctNo}", _context.Settings.FirstOrDefault(s => s.Name == AccountNumber)?.Value);

            return message;
        }

        public string GetCashPaymentSuccessfulMail(Booking booking)
        {
            var message = GetFileContent("Cash_Payment_Successful.html");
            message = message.Replace("{Name}", $"{booking.User?.FirstName} {booking.User?.LastName}");
            message = message.Replace("{TransRef}", booking.TransactionRef);
            message = message.Replace("{Date}", DateTime.Now.ToShortDateString());
            message = message.Replace("{Time}", DateTime.Now.ToShortTimeString());
            message = message.Replace("{Title}", booking.Activity?.Title);
            message = message.Replace("{Amount}", string.Format("{0:#,##0}", booking.Activity.Amount));
            message = message.Replace("{AmountPaid}", string.Format("{0:#,##0}", booking.Amount));

            return message;
        }

        public async Task SendCashBookingSuccessfulEmailAsync(int bookingId)
        {
            try
            {
                var booking = await _context.Bookings
                            .Include(b => b.User)
                            .Include(b => b.Activity)
                            .Include("Activity.ActivityType")
                            .FirstOrDefaultAsync(m => m.Id == bookingId);
                var message = GetCashPaymentSuccessfulMail(booking);
                await _emailSender.SendEmailAsync(booking.User?.Email, $"{booking?.Activity?.ActivityType?.Name} Payment Successful", message);
            }
            catch (Exception e)
            { }
        }

        private string GetFileContent(string fileName)
        {
            var filePath = $"{_templateDir}{fileName}";
            var message = string.Empty;
            using (StreamReader reader = File.OpenText(filePath))
            {
                message = reader.ReadToEnd();
            }
            return message;
        }
    }
}
