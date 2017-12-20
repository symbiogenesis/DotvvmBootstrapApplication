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
            var data = new List<ExampleRecord>
                        {
                            new ExampleRecord{ Name = "AAA" },
                            new ExampleRecord{ Name = "BBB" },
                            new ExampleRecord{ Name = "CCC" }
                        };

            // Create a mock set and context
            var set = new Mock<DbSet<ExampleRecord>>();

            var context = new Mock<RingDownConsoleDbContext>();
            context.Setup(c => c.ExampleTable).Returns(set.Object);

            _context = context.Object;
        }

        [Fact]
        public void GetTable_ExampleTableNotNull()
        {
            var table = _context.GetTable<ExampleRecord>();

            Assert.NotNull(table);
        }

        [Fact]
        public void GetTable_ExampleTableNotEmpty()
        {
            var table = _context.GetTable<ExampleRecord>();

            Assert.NotEmpty(table);
        }

        [Fact]
        public void GetTable_ExampleTableFound()
        {
            var table = _context.GetTable<ExampleRecord>();

            Assert.Equal(_context.ExampleTable, table);
        }
    }
}
