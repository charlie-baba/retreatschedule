using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RetreatSchedule.Data;
using RetreatSchedule.Models.Enum;
using RetreatSchedule.Poco;
using RetreatSchedule.Util;
using RetreatSchedule.ViewModel;

namespace RetreatSchedule.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly RetreatDBContext _context;
        private const string _onlineColor = "#006cff";
        private const string _cashColor = "#00cbff";
        private const string _greenColor = "#04c142";

        public DashboardController(RetreatDBContext context)
        {
            _context = context;
        }

        // GET: Dashboard
        [HttpGet]
        public ActionResult Index()
        {
            var totalVisits = _context.Visitors.Count(x => x.Page == Page.Home && x.Date.Year == DateTime.Now.Year);
            var totalBookings = _context.Bookings.Count(x => x.PaymentStatus == PaymentStatus.Successful && x.DateCreated.Year == DateTime.Now.Year);
            var onlinePayments = _context.Bookings
                .Count(x => x.PaymentStatus == PaymentStatus.Successful && x.PaymentType == PaymentType.Online && x.DateCreated.Year == DateTime.Now.Year);
            var cashPayments = _context.Bookings
                .Count(x => x.PaymentStatus == PaymentStatus.Successful && x.PaymentType == PaymentType.Cash && x.DateCreated.Year == DateTime.Now.Year);
            var data = new DashboardViewModel() {
                TotalVisits = totalVisits,
                TotalBookings = totalBookings,
                OnlinePayments = onlinePayments,
                CashPayments = cashPayments
            };
            return View(data);
        }

        [HttpGet]
        public IActionResult ChartData()
        {
            var response = new ChartsData
            {
                DonutChartDatas = new List<DonutChartData>
                {
                    new DonutChartData{ Label = "Online Payments", Color = _onlineColor,
                        Value = _context.Bookings
                            .Count(x => x.PaymentType == PaymentType.Online && x.PaymentStatus == PaymentStatus.Successful && x.DateCreated.Year == DateTime.Now.Year)},
                    new DonutChartData{ Label = "Cash Payments", Color = _cashColor,
                        Value = _context.Bookings
                            .Count(x => x.PaymentType == PaymentType.Cash && x.PaymentStatus == PaymentStatus.Successful && x.DateCreated.Year == DateTime.Now.Year)},
                },
                AreaChartDatas = new List<AreaChartData>
                {
                    new AreaChartData
                    {
                        Key = "Payment",
                        Color = _onlineColor,
                        Values = _context.Bookings
                            .Where(x => x.PaymentStatus == PaymentStatus.Successful && x.DateCreated.Year == DateTime.Now.Year)
                            .GroupBy(x => x.DateCreated.Date)
                            .Select(x => new double[] { x.Key.ToJsTime(), x.Count() }).ToList()
                    }
                },
                VisitorChartDatas = new List<AreaChartData>
                {
                    new AreaChartData
                    {
                        Key = "Visitors",
                        Color = _greenColor,
                        Values = _context.Visitors
                            .Where(x => x.Page == Page.Home && x.Date.Year == DateTime.Now.Year)
                            .GroupBy(x => x.Date.Date)
                            .Select(x => new double[] { x.Key.ToJsTime(), x.Count() }).ToList()
                    }
                },
                CalendarChartDatas = _context.Activities
                        .Where(x => x.StartDate.Year == DateTime.Now.Year)
                        .Select(x => new string[] { x.StartDate.ToString("d/M/yyyy"), x.Title, $"/Home/Details/{x.Id}", _cashColor })
                        .ToArray()
            };

            //var data = _context.Visitors
            //                .Where(x => x.Page == Page.Home && x.Date.Year == DateTime.Now.Year)
            //                .GroupBy(x => x.Date.Date)
            //                .Select(x => new { Name = x.Key, Count = x.Count() })
            //                .OrderBy(x => x.Name).ToList();
            //var item = data;
            return Json(response);
        }
    }
}