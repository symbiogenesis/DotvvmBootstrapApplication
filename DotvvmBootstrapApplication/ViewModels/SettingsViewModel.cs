using System.Threading.Tasks;
using DotVVM.Framework.Runtime.Filters;
using Microsoft.Extensions.Options;
using DotvvmBootstrapApplication.Models;
using DotvvmBootstrapApplication.Models.Enums;
using DotvvmBootstrapApplication.Services;

namespace DotvvmBootstrapApplication.ViewModels
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
