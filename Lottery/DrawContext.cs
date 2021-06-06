using Microsoft.EntityFrameworkCore;

namespace Lottery
{
    public class DrawContext : DbContext
    {
        public DbSet<Draw> Draw { get; set; }
    }
}
