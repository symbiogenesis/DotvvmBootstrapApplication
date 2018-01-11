using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RingDownConsole.Models;

namespace RingDownConsole.WebAPI.Controllers
{
    public class LocationController : BaseController<Location>
    {
        public LocationController(RingDownConsoleDbContext context) : base(context)
        {
        }

        [HttpGet("getlocation/{id}")]
        public async Task<Location> GetBySerialNumber(string serialNumber)
        {
            var location = _context.Locations.FirstOrDefault(l => l.SerialNumber == serialNumber);

            if (location == default(Location))
            {
                var maxId = _context.Locations.Max(l => l.Id);

                maxId++;

                var newLocation = new Location
                {
                    Id = maxId,
                    SerialNumber = serialNumber,
                    Code = maxId.ToString(),
                    Name = maxId.ToString()
                };

                await _context.Locations.AddAsync(newLocation);
                await _context.SaveChangesAsync();
            }

            return location;
        }
    }
}
