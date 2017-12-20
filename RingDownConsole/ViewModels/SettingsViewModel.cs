using System.Threading.Tasks;
using DotVVM.Framework.Runtime.Filters;
using Microsoft.Extensions.Options;
using RingDownConsole.Models;
using RingDownConsole.Models.Enums;
using RingDownConsole.Services;

namespace RingDownConsole.ViewModels
{
    [Authorize(Policy = nameof(Roles.Administrator))]
    public class SettingsViewModel : MasterViewModel
    {
        private readonly SettingsService _settingsService;

        public SettingsViewModel(
            IOptionsSnapshot<AppSettings> appSettings,
            SettingsService settingsService) : base(appSettings)
        {
            PageTitle = "Settings";

            _settingsService = settingsService;
        }
    }
}
