using RingDownConsole.Models;

namespace RingDownConsole.WebAPI.Controllers
{
    public class StatusController : BaseController<Status>
    {
        public StatusController(RingDownConsoleDbContext context) : base(context)
        {
        }
    }
}
