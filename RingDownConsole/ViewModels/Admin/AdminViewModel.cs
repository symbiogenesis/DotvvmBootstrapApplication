using DotVVM.Framework.Controls;
using DotVVM.Framework.Runtime.Filters;
using DotVVM.Framework.ViewModel;
using Microsoft.Extensions.Options;
using RingDownConsole.Interfaces;
using RingDownConsole.Models;
using RingDownConsole.Models.Enums;
using RingDownConsole.Utils.Extensions;
using TrackableEntities.Common.Core;

namespace RingDownConsole.ViewModels.Admin
{
    [Authorize(Policy = nameof(Roles.Administrator))]
    public class AdminViewModel<T> : MasterViewModel where T : class, IAdminLookup, ITrackable, IMergeable
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
