using Microsoft.EntityFrameworkCore;
using Workshop.Models;

namespace Workshop.DB
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options)
         : base(options)
        {
        }


        public DbSet<Models.Workshop> Workshop { get; set; }
        public DbSet<Usuario> Usuario { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Database=WorkshopDB;Username=postgres;Password=123");
        }

    }
}
