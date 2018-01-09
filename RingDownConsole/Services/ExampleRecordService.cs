using System;
using System.Linq;
using System.Threading.Tasks;
using RingDownConsole.Models;
using RingDownConsole.Utils.Extensions;

namespace RingDownConsole.Services
{
    public class ExampleRecordService
    {
        private readonly RingDownConsoleDbContext _context;

        public ExampleRecordService(RingDownConsoleDbContext context)
        {
            _context = context;
        }

        internal async Task Submit(ExampleRecord record)
        {
            await _context.AddAsync(record);

            await _context.SaveChangesAsync();
        }

        internal ExampleRecord Get(int id)
        {
            return _context.ExampleTable.First(t => t.Id == id);
        }

        internal IQueryable<ExampleRecord> GetRecordRange(int maxAge)
        {
            return _context.ExampleTable
                .Where(t => t.LoggedDateTime.IsBetween(maxAge));
        }
    }
}
