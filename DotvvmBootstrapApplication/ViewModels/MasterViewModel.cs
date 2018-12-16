using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DotVVM.Framework.Hosting;
using DotVVM.Framework.ViewModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using DotvvmBootstrapApplication.ActionFilters;
using DotvvmBootstrapApplication.Interfaces;
using DotvvmBootstrapApplication.Models;
using DotvvmBootstrapApplication.Models.Enums;
using DotvvmBootstrapApplication.Utils.Extensions;
using Serilog;

namespace DotvvmBootstrapApplication.ViewModels
{
    [ViewModelLoggingFilter]
    public class MasterViewModel : DotvvmViewModelBase, IViewModel
    {
        protected readonly AppSettings _appSettings;

        protected bool _isAdmin => Context.HttpContext.User.IsInRole(nameof(Roles.Administrator));

        [Bind(Direction.ServerToClientFirstRequest)]
        public string UserName => Context.HttpContext.User.Identity.Name;

        [Bind(Direction.ServerToClientFirstRequest)]
        public string SiteTitle => _appSettings.ApplicationName;

        [Bind(Direction.ServerToClientFirstRequest)]
        public virtual string PageTitle { get; set; }

        [Bind(Direction.ServerToClientFirstRequest)]
        public virtual string ErrorTitle => "Error:";

        [Bind(Direction.ServerToClient)]
        public string ErrorMessage { get; set; }

        [Bind(Direction.ServerToClient)]
        public bool ErrorShow { get; set; }

        [Bind(Direction.ServerToClientFirstRequest)]
        public virtual bool ShowLogout => true;

        [Bind(Direction.ServerToClientFirstRequest)]
        public virtual bool ShowPageTitle => true;

        public MasterViewModel(IOptionsSnapshot<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public override Task Load()
        {
            HideError();

            return base.Load();
        }

        public virtual async Task SignIn()
        {
            GoToRoute(Routes.Login);
        }

        public async Task SignOut()
        {
            await Context.GetAuthentication().SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            GoToRoute(Routes.Example);
        }

        public void GoToRoute(Routes route, string queryString = null)
        {
            Context.RedirectToRoute(route.ToString(), null, false, true, queryString);
        }

        public void DownloadFile(string fullPath, string mime)
        {
            Context.ReturnFile(File.ReadAllBytes(fullPath), Path.GetFileName(fullPath), mime);
        }

        public void ShowError(string message)
        {
            ErrorMessage = message;
            ErrorShow = true;
        }

        public void HideError()
        {
            ErrorMessage = null;
            ErrorShow = false;
        }

        protected void LogError(Exception ex)
        {
            Log.Logger.Error(ex, ex.GetErrorMessage());
        }

        protected void LogInfo(string message)
        {
            Log.Logger.Information(message);
        }

        protected IQueryable<DashboardDTO> ConvertToDTO(IQueryable<ExampleRecord> queryable)
        {
            return queryable.Select(record => new DashboardDTO
            {
                RecordId = record.Id.ToString(),
                Name = record.Name,
                RecordNum = record.RecordNum
            });
        }
    }
}

