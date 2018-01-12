using System;
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

        [HttpGet("getlocation/{serialNumber}")]
        public async Task<Location> GetBySerialNumber(string serialNumber)
        {
            var any = _context.Locations.Any();

            if (!any)
            {
                return await AddLocation(serialNumber, any);
            }

            var location = _context.Locations.FirstOrDefault(l => l.SerialNumber == serialNumber);

            if (location == default(Location))
            {
                return await AddLocation(serialNumber, any);
            }
            else
            {
                return location;
            }
        }

        private async Task<Location> AddLocation(string serialNumber, bool any)
        {
            var maxId = GetHighestLocationId(any);

            maxId++;

            var newLocation = new Location
            {
                Id = maxId,
                SerialNumber = serialNumber,
                Code = serialNumber,
                Name = serialNumber
            };

            await _context.Locations.AddAsync(newLocation);
            await _context.SaveChangesAsync();

            return newLocation;
        }

        private int GetHighestLocationId(bool any)
        {
            if (any)
                return _context.Locations.Max(l => l.Id);
            else
                return 1;
        }
    }
}
