using BookingAudience.Models;
using BookingAudience.Models.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookingAudience.DAL
{
    public class ApplicationDBContext : IdentityDbContext<AppUser, UserRole, int>
    {
        public DbSet<AppUser> Users { get; set; }
        public DbSet<Audience> Audiences { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
           : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<AppUser>();
            builder.Entity<Audience>();
            builder.Entity<Booking>();
            builder.Entity<Building>();
            base.OnModelCreating(builder);
        }
    }
}
