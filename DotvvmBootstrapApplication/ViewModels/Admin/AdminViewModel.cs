using DotVVM.Framework.Controls;
using DotVVM.Framework.Runtime.Filters;
using DotVVM.Framework.ViewModel;
using Microsoft.Extensions.Options;
using DotvvmBootstrapApplication.Interfaces;
using DotvvmBootstrapApplication.Models;
using DotvvmBootstrapApplication.Models.Enums;
using DotvvmBootstrapApplication.Utils.Extensions;

namespace DotvvmBootstrapApplication.ViewModels.Admin
{
    [Authorize(Policy = nameof(Roles.Administrator))]
    public class AdminViewModel<T> : MasterViewModel where T : class, IAdminLookup
    {
        public GridViewDataSet<T> Data { get; set; }

        public bool IsNotEditing => Data?.RowEditOptions?.EditRowId == null;

        [Bind(Direction.ServerToClientFirstRequest)]
        public string DataType => typeof(T).Name.SplitCamelCase();

        [Bind(Direction.ServerToClientFirstRequest)]
        public override string PageTitle => $"Admin > {DataType}s";

        public AdminViewModel(
            IOptionsSnapshot<AppSettings> appSettings) : base(appSettings)
        {
        }
    }
}
