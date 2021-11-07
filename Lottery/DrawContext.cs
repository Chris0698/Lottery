using Microsoft.EntityFrameworkCore;

namespace Lottery
{
    public class DrawContext : DbContext
    {
        private string connectionString = "Server=(localdb)\\mssqllocaldb;Database=Draws;Trusted_Connection=True;";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }

        public DbSet<Draw> Draw { get; set; }
    }
}
