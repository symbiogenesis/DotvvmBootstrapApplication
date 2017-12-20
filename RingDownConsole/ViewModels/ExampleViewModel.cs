using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DotVVM.Framework.Controls;
using DotVVM.Framework.Runtime.Filters;
using DotVVM.Framework.ViewModel;
using Microsoft.Extensions.Options;
using RingDownConsole.Models;
using RingDownConsole.Models.Enums;
using RingDownConsole.Services;
using RingDownConsole.Utils.Extensions;

namespace RingDownConsole.ViewModels
{
    [Authorize(Policy = nameof(Roles.User))]
    public class ExampleViewModel : MasterViewModel
    {
        private readonly ExampleRecordService _exampleRecordService;
        private static readonly IEnumerable<PropertyInfo> _filterStringProperties = typeof(DashboardDTO).GetProperties().Where(p => p.PropertyType == typeof(string));
        private static readonly IEnumerable<PropertyInfo> _filterListProperties = typeof(Filter).GetProperties().Where(p => p.PropertyType == typeof(IEnumerable<string>));

        public enum SpreadsheetType
        {
            CSV = 0,
            Excel = 1
        }

        public bool ShowPending { get; }

        [Bind(Direction.ServerToClientFirstRequest)]
        public override bool ShowPageTitle => false;

        public Filter Filter { get; set; } = new Filter();

        public bool IsFiltered { get; private set; }

        public GridViewDataSet<DashboardDTO> Data { get; set; }

        public ExampleViewModel(
            IOptionsSnapshot<AppSettings> appSettings,
            ExampleRecordService exampleRecordService) : base(appSettings)
        {
            PageTitle = "Example";

            _exampleRecordService = exampleRecordService;
        }

        public async Task ClearFilters()
        {
            GoToRoute(Routes.ExampleView);
        }

        private GridViewDataSetLoadedData<DashboardDTO> GetData(IGridViewDataSetLoadOptions options)
        {
            var queryable = GetQueryable();

            if (queryable == null)
                throw new Exception($"Ring Down Console data could not be retrieved from server. Check your connection and try again.");

            ApplyDefaultSorting(options.SortingOptions);

            try
            {
                return queryable.GetDataFromQueryable(options);
            }
            catch (InvalidOperationException e)
            {
                ShowError("No records found");
                return new GridViewDataSetLoadedData<DashboardDTO>(new List<DashboardDTO>(), 0);
            }
        }

        private void ApplyDefaultSorting(ISortingOptions sortingOptions)
        {
            if (sortingOptions.SortExpression == null)
            {
                sortingOptions.SortExpression = nameof(Filter.DateTimeValue);
                sortingOptions.SortDescending = true;
            }
        }

        private void ForceDefaultSorting()
        {
            const string logDateTimeValue = nameof(Filter.DateTimeValue);

            if (Data.SortingOptions.SortExpression == logDateTimeValue && Data.SortingOptions.SortDescending)
                return;

            Data.SetSortExpression(logDateTimeValue);

            if (Data.SortingOptions.SortDescending)
                return;

            ForceDefaultSorting();
        }

        private IQueryable<DashboardDTO> GetQueryable()
        {
            var queryable = _exampleRecordService.GetRecordRange(_appSettings.MaxAgeForDashboardLogs);

            var dashboardItems = ConvertToDTO(queryable);

            IsFiltered = false;

            return FilterDashboard(dashboardItems);
        }

        private IQueryable<DashboardDTO> FilterDashboard(IQueryable<DashboardDTO> dashboardItems)
        {
            foreach (var property in _filterStringProperties)
            {
                if (string.IsNullOrWhiteSpace(property.GetString(Filter)))
                    continue;

                dashboardItems = dashboardItems.Where(g => property.GetString(g).IndexOf(property.GetString(Filter), StringComparison.OrdinalIgnoreCase) >= 0);
                IsFiltered = true;
            }

            foreach (var property in _filterListProperties)
            {
                var list = property.GetList(Filter);

                if (!list.Any())
                    continue;

                var guestProperty = _filterStringProperties.First(p => p.Name == property.Name.TrimEnd('s'));

                dashboardItems = from g in dashboardItems join p in list on guestProperty.GetString(g) equals p select g;

                IsFiltered = true;
            }

            return dashboardItems;
        }

        public override Task Init()
        {
            if (Filter == null)
            {
                Filter = new Filter();
            }

            // NOTE: You can also create the DataSet with factory.
            // Just call static Create with delegate and pagesize.            
            Data = GridViewDataSet.Create(gridViewDataSetLoadDelegate: GetData, pageSize: _appSettings.PageSize);

            return base.Init();
        }

        public async Task DateFilterChanged()
        {
            Data.LoadFromQueryable(GetQueryable());
        }
    }
}
