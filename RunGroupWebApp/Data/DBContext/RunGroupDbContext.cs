using Microsoft.EntityFrameworkCore;
using RunGroupWebApp.Models;

namespace RunGroupWebApp.Data.DBContext
{
    public class RunGroupDbContext : DbContext
    {
        public RunGroupDbContext(DbContextOptions<RunGroupDbContext> options) : base(options)
        {
            
        }

        public DbSet<Race> Races { get; set; }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<Club> Clubs { get; set; }

        public DbSet<AppUser> AppUsers { get; set; }

    }
}
