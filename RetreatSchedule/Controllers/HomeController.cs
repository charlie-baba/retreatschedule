using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RetreatSchedule.Config;
using RetreatSchedule.Data;
using RetreatSchedule.Models;
using RetreatSchedule.Models.Enum;
using RetreatSchedule.Poco;
using RetreatSchedule.Util;
using RetreatSchedule.ViewModel;
using DiagnosticActivity = System.Diagnostics.Activity;

namespace RetreatSchedule.Controllers
{
    public class HomeController : Controller
    {
        private readonly RetreatDBContext _context;
        private const int pageSize = 10;
        private readonly PaystackConfig _config;

        public HomeController(RetreatDBContext context, IOptions<PaystackConfig> paystackConfig)
        {
            _context = context;
            _config = paystackConfig?.Value;
        }

        public async Task<IActionResult> Landing()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? page, string locationName, string activityName)
        {
            ViewData["AcctDetails"] = string.Empty;
            ViewData["LocationName"] = locationName;
            ViewData["ActivityName"] = activityName;
            ViewData["PublicKey"] = _config?.PublicKey;
            var model = new HomeViewModel();
            var activities = _context.Activities
                .Include(a => a.Location)
                .Include(a => a.Bookings)
                .OrderBy(a => a.StartDate)
                .Where(a => a.EndDate > DateTime.Now);
            if (!string.IsNullOrWhiteSpace(locationName))
                activities = activities.Where(a => a.Location.Name == locationName);
            if (!string.IsNullOrWhiteSpace(activityName))
                activities = activities.Where(a => a.ActivityType.Name == activityName);

            model.Activities = await PaginatedList<Activity>.CreateAsync(activities.AsNoTracking(), page ?? 1, pageSize);

            var locations = _context.Activities
                .Where(a => a.EndDate > DateTime.Now)
                .GroupBy(x => x.Location.Name)
                .Select(x => new CountData { Name = x.Key, Count = x.Count() });
            model.Locations = await locations.ToListAsync();

            var activityTypes = _context.Activities
                .Where(a => a.EndDate > DateTime.Now)
                .GroupBy(x => x.ActivityType)
                .Select(x => new CountData { Name = x.Key.Name, Count = x.Count() });
            model.ActivityTypes = await activityTypes.ToListAsync();

            if (page == null)
                await LogVisit();

            return View(model);
        }

        private async Task LogVisit()
        {
            var user = HttpContext.Session.Get<User>(Constants.SessionKeyUser);
            var remoteIpAddress = HttpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress.ToString();
            var visitor = new Visitor() { Date = DateTime.Now, Page = Page.Home, IpAddress = remoteIpAddress, UserId = user?.Id };
            _context.Visitors.Add(visitor);
            await _context.SaveChangesAsync();
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _context.Activities
                .Include(a => a.ActivityType)
                .Include(a => a.Location)
                .Include(a => a.Bookings)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (activity == null)
            {
                return NotFound();
            }

            ViewData["AcctDetails"] = string.Empty;
            ViewData["MapUrl"] = activity.Location.MapUrl;
            var supportEmail = _context.Settings.FirstOrDefault(x => x.Name == SettingConstants.SupportEmail);
            var supportPhone = _context.Settings.FirstOrDefault(x => x.Name == SettingConstants.SupportPhone);
            ViewData["ContactEmail"] = supportEmail?.Value;
            ViewData["ContactPhone"] = supportPhone?.Value;
            ViewData["PublicKey"] = _config?.PublicKey;
            return View(activity);
        }

        [HttpGet]
        public async Task<IActionResult> Payment(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var activity = await _context.Activities.Include(x => x.Location)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (activity == null || !activity.IsActive)
            {
                return NotFound();
            }

            ViewData["ReferringCentreID"] = new SelectList(_context.Centres, "Id", "Name");
            ViewData["PublicKey"] = _config?.PublicKey;
            ViewData["ActivityID"] = id;
            var isIwollo = activity.Location?.Name?.IndexOf("Iwollo", StringComparison.OrdinalIgnoreCase) > -1;
            ViewData["IsIwollo"] = isIwollo;
            var accountDetails = string.Empty;
            if (isIwollo)
                accountDetails = "<span>Account Name: <b>Wetland Cultural and Education Foundation - Iwollo Booking</b></span> <br/> <span>Account Number: <b>0809077029</b></span>";
            else
                accountDetails = "<span>Account Name: <b>Wetland Booking</b></span> <br/> <span>Account Number: <b>0768518344</b></span>";
            ViewData["AcctDetails"] = accountDetails;
            return View();
        }

        public IActionResult Privacy()
        {
            ViewData["AcctDetails"] = string.Empty;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = DiagnosticActivity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
