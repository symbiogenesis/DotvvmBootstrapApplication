using RingDownConsole.Models;

namespace RingDownConsole.Services
{
    public class SettingsService
    {
        private readonly RingDownConsoleDbContext _context;

        public SettingsService(RingDownConsoleDbContext context)
        {
            _context = context;
        }
    }
}
