using DotvvmBootstrapApplication.Models;

namespace DotvvmBootstrapApplication.Services
{
    public class SettingsService
    {
        private readonly BootstrapDbContext _context;

        public SettingsService(BootstrapDbContext context)
        {
            _context = context;
        }
    }
}
