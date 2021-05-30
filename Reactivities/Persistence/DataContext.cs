using Reactivities.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Domain;

namespace Reactivities.Persistence
{
    public class DataContext : IdentityDbContext<AppUser>
    {
        public DataContext(DbContextOptions options) : base(options) {

        }

        public DbSet<Activity> Activities { get; set; }

    }
}