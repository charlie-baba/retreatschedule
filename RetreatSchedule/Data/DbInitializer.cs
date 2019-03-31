using System.Linq;
using RetreatSchedule.Data;
using RetreatSchedule.Models;
using static RetreatSchedule.Util.SettingConstants;

namespace RechargeKad.Data
{
    public static class DbInitializer
    {
        public static void Initialize(RetreatDBContext context)
        {
            context.Database.EnsureCreated();

            if (!context.ActivityTypes.Any())
            {
                var activityTypes = new ActivityType[]
                {
                    new ActivityType {Name = "Retreat", Description = "Retreat"},
                    new ActivityType {Name = "Workshop", Description = "Workshop"}
                };
                context.ActivityTypes.AddRange(activityTypes);
            }

            if (!context.Settings.Any())
            {
                var settings = new Setting[]
                {
                    new Setting { Name = SupportEmail, Value = "bookings@retreatschedule.com, bookings@wetland.org.ng" },
                    new Setting { Name = SupportPhone, Value = "08020577959" },
                    new Setting { Name = BankName, Value = "Access Bank" },
                    new Setting { Name = AccountName, Value = "Wetland Booking" },
                    new Setting { Name = AccountNumber, Value = "0768518344" },
                };
                context.Settings.AddRange(settings);
            }

            if (!context.Centres.Any())
            {
                var centres = new Centre[]
                {
                    new Centre { Name = "Abuja" },
                    new Centre { Name = "Ekulu" },
                    new Centre { Name = "Eleko" },
                    new Centre { Name = "Helmbridge" },
                    new Centre { Name = "Irawo" },
                    new Centre { Name = "Ugwuoma" },
                    new Centre { Name = "VI Centre" }
                };
                context.Centres.AddRange(centres);
            }
            context.SaveChanges();
        }
    }

}