using Microsoft.EntityFrameworkCore;

namespace Geocaches.Models
{
    public class GeocacheContext : DbContext
    {
        public GeocacheContext(DbContextOptions<GeocacheContext> options) : base(options)
        {

        }

        public DbSet<GeocacheItem> GeocacheItems { get; set; }
        public DbSet<Geocache> Geocaches { get; set; }
    }
}

