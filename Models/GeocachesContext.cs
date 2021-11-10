using Microsoft.EntityFrameworkCore;

namespace Geocaches.Models
{
    public class GeocachesContext : DbContext
    {
        public GeocachesContext(DbContextOptions<GeocachesContext> options) : base(options)
        {
        }

        public DbSet<GeocacheItem> GeocacheItems { get; set; }
        public DbSet<Geocache> Geocaches { get; set; }
    }
}