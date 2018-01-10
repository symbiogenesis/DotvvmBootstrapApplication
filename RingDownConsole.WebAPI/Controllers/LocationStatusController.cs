using RingDownConsole.Models;

namespace RingDownConsole.WebAPI.Controllers
{
    public class LocationStatusController : BaseController<LocationStatus>
    {
        public LocationStatusController(RingDownConsoleDbContext context) : base(context)
        {
        }
    }
}
