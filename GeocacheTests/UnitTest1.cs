
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Xunit;

namespace Geocaches.GeocacheTests
{
    public class UnitTest1
    {
        private GeocachesContext _context;

        public UnitTest1()
        {
            DbContextOptions<GeocachesContext> dbOptions = new DbContextOptionsBuilder<GeocachesContext>().UseInMemoryDatabase("TestDb").Options;
            _context = new GeocachesContext(dbOptions);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            if (!_context.Geocaches.Any())
            {
                _context.Geocaches.Add(new Geocache() { Id = 1, Name = "First cache" });
                _context.Geocaches.Add(new Geocache() { Id = 2, Name = "Second cache" });
                _context.SaveChanges();
            }
            geocaches = new GeocacheController(null, _context);

            if (!_context.GeocacheItems.Any())
            {
                _context.GeocacheItems.Add(new GeocacheItem() { Id = 1, Name = "First Item" });
                _context.GeocacheItems.Add(new GeocacheItem() { Id = 2, Name = "Second Item" });
                _context.SaveChanges();
            }
            geocachesItems = new GeocacheItemController(null, _context);
        }
        private readonly GeocacheController geocaches;
        private readonly GeocacheItemController geocachesItems;

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        // [InlineData(3)]

        public void Passing_Geocache_Exists_Tests(int value)
        {
            // assert statement, Assert.Equal(What you think the answer should be, What method is being called, with the parameters)

            bool geocache = geocaches.GeocacheExist(value);
            Assert.True(geocache);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        // [InlineData(3)]

        public void Passing_GeocacheItem_Exists_Tests(int value)
        {
            // assert statement, Assert.Equal(What you think the answer should be, What method is being called, with the parameters)

            bool geocacheItem = geocachesItems.GeocacheItemExists(value);
            Assert.True(geocacheItem);
        }


    }
}
