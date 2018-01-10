using System.Collections.Generic;
using RingDownConsole.Models;
using RingDownConsole.Utils.Extensions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace RingDownConsole.Tests
{
    public class DbContextExtensionTests
    {
        private readonly RingDownConsoleDbContext _context;

        public DbContextExtensionTests()
        {
            var data = new List<LocationStatus>
                        {
                            new LocationStatus{ CurrentPhoneUser = "AAA" },
                            new LocationStatus{ CurrentPhoneUser = "BBB" },
                            new LocationStatus{ CurrentPhoneUser = "CCC" }
                        };

            // Create a mock set and context
            var set = new Mock<DbSet<LocationStatus>>();

            var context = new Mock<RingDownConsoleDbContext>();
            context.Setup(c => c.LocationStatuses).Returns(set.Object);

            _context = context.Object;
        }

        [Fact]
        public void GetTable_ExampleTableNotNull()
        {
            var table = _context.GetTable<LocationStatus>();

            Assert.NotNull(table);
        }

        [Fact]
        public void GetTable_ExampleTableNotEmpty()
        {
            var table = _context.GetTable<LocationStatus>();

            Assert.NotEmpty(table);
        }

        [Fact]
        public void GetTable_ExampleTableFound()
        {
            var table = _context.GetTable<LocationStatus>();

            Assert.Equal(_context.LocationStatuses, table);
        }
    }
}
