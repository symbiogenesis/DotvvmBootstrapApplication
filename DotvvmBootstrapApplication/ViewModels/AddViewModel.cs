using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DotVVM.Framework.Controls;
using DotVVM.Framework.Runtime.Filters;
using DotVVM.Framework.ViewModel;
using Microsoft.Extensions.Options;
using DotvvmBootstrapApplication.Interfaces;
using DotvvmBootstrapApplication.Models;
using DotvvmBootstrapApplication.Models.Enums;
using DotvvmBootstrapApplication.Services;
using DotvvmBootstrapApplication.Utils.Extensions;

namespace DotvvmBootstrapApplication.ViewModels
{
    [Authorize(Policy = nameof(Roles.User))]
    public class AddViewModel : MasterViewModel
    {
        private readonly ExampleRecordService _exampleRecordService;
        public ExampleRecord RecordToBeAdded { get; set; } = new ExampleRecord();

        public AddViewModel(
            IOptionsSnapshot<AppSettings> appSettings,
            ExampleRecordService exampleRecordService) : base(appSettings)
        {
            PageTitle = "Add Record";

            _exampleRecordService = exampleRecordService;
        }

        public void Save()
        {
            _exampleRecordService.Submit(RecordToBeAdded);
            GoToRoute(Routes.Example);
        }
    }
}
