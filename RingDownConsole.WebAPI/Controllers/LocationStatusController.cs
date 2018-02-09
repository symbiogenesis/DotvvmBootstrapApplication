using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RingDownConsole.Models;

namespace RingDownConsole.WebAPI.Controllers
{
    public class LocationStatusController : BaseController<LocationStatus>
    {
        public LocationStatusController(RingDownConsoleDbContext context) : base(context)
        {
        }

        [HttpPost]
        public override async Task Post([FromBody]LocationStatus record)
        {
            // For more information on protecting this API from Cross Site Request Forgery (CSRF) attacks, see https://go.microsoft.com/fwlink/?LinkID=717803

            var lastStatus = _context.LocationStatuses
                                        .Where(ls => ls.LocationId == record.LocationId)
                                        .OrderByDescending(ls => ls.RecordedDate)
                                        .FirstOrDefault();

            if (lastStatus == null)
            {
                await base.Post(record);
                return;
            }

            if (lastStatus.StatusId != record.StatusId || lastStatus.RecordedDate < DateTime.UtcNow.AddMinutes(-10))
                await base.Post(record);
        }
    }
}
