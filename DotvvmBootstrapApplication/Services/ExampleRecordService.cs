using System;
using System.Linq;
using System.Threading.Tasks;
using DotvvmBootstrapApplication.Models;
using DotvvmBootstrapApplication.Utils.Extensions;

namespace DotvvmBootstrapApplication.Services
{
    public class ExampleRecordService
    {
        private readonly BootstrapDbContext _context;

        public ExampleRecordService(BootstrapDbContext context)
        {
            _context = context;
        }

        internal async Task Submit(ExampleRecord record)
        {
            await _context.AddAsync(record);

            await _context.SaveChangesAsync();
        }

        internal ExampleRecord Get(Guid id)
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
