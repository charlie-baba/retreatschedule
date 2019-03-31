using Microsoft.EntityFrameworkCore;
using RetreatSchedule.Models;

namespace RetreatSchedule.Data
{
    public class RetreatDBContext : DbContext
    {
        public RetreatDBContext(DbContextOptions<RetreatDBContext> options) : base(options) { }

        public RetreatDBContext() { }

        public DbSet<Activity> Activities { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Location> Locations { get; set; }

        public DbSet<Booking> Bookings { get; set; }

        public DbSet<ActivityType> ActivityTypes { get; set; }

        public DbSet<Setting> Settings { get; set; }

        public DbSet<Visitor> Visitors { get; set; }

        public DbSet<Centre> Centres { get; set; }

        public DbSet<Donation> Donations { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>().HasIndex(u => u.Email).IsUnique();
            builder.Entity<Location>().HasIndex(u => u.Name).IsUnique();
            builder.Entity<Booking>().HasIndex(u => u.TransactionRef).IsUnique();
            //builder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
