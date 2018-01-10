using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RingDownConsole.Models;
using RingDownConsole.Utils.Extensions;

namespace RingDownConsole.Services
{
    public class LocationStatusService
    {
        private readonly RingDownConsoleDbContext _context;

        public LocationStatusService(RingDownConsoleDbContext context)
        {
            _context = context;
        }

        internal async Task Submit(LocationStatus record)
        {
            await _context.AddAsync(record);

            await _context.SaveChangesAsync();
        }

        internal LocationStatus Get(int id)
        {
            return _context.LocationStatuses
                            .Include(ls => ls.Location)
                            .Include(ls => ls.Status)
                            .First(t => t.Id == id);
        }

        internal IQueryable<LocationStatus> GetRecordRange(int maxAge)
        {
            return _context.LocationStatuses
                            .Include(ls => ls.Location)
                            .Include(ls => ls.Status)
                            .Where(t => t.RecordedDate.IsBetween(maxAge));
        }
    }
}
