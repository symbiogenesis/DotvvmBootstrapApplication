using RingDownConsole.Models;

namespace RingDownConsole.WebAPI.Controllers
{
    public class LocationController : BaseController<Location>
    {
        public LocationController(RingDownConsoleDbContext context) : base(context)
        {
        }
    }
}
