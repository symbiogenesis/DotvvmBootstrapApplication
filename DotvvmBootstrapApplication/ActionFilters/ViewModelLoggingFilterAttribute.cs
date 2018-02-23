using System;
using System.Threading.Tasks;
using DotVVM.Framework.Hosting;
using DotVVM.Framework.Runtime.Filters;
using DotvvmBootstrapApplication.Interfaces;
using DotvvmBootstrapApplication.Utils.Extensions;
using Serilog;

namespace DotvvmBootstrapApplication.ActionFilters
{
    [AttributeUsageAttribute(AttributeTargets.All, AllowMultiple = false)]
    public class ViewModelLoggingFilterAttribute : ExceptionFilterAttribute
    {
        protected override Task OnCommandExceptionAsync(IDotvvmRequestContext context, ActionInfo actionInfo, Exception ex)
        {
            LogError(context.CommandException);

            // AppViewModelBase declares the ErrorMessage property to display error messages
            // If it is set, the master page will display the error message alert.
            if (context.ViewModel is IViewModel)
            {
                ((IViewModel)context.ViewModel).ErrorShow = true;
                ((IViewModel)context.ViewModel).ErrorMessage = ex.Message;

                // We need the request to end normally, not with an error
                context.IsCommandExceptionHandled = true;
            }

            return base.OnCommandExceptionAsync(context, actionInfo, ex);
        }

        protected override Task OnPageExceptionAsync(IDotvvmRequestContext context, Exception ex)
        {
            LogError(ex);

            if (context.ViewModel is IViewModel && context.IsSpaRequest)
            {
                ((IViewModel)context.ViewModel).ErrorShow = true;
                ((IViewModel)context.ViewModel).ErrorMessage = ex.Message;

                // We need the request to end normally, not with an error
                context.IsPageExceptionHandled = true;
            }

            return base.OnPageExceptionAsync(context, ex);
        }

        private void LogError(Exception ex)
        {
            Log.Logger.Error(ex, ex.GetErrorMessage());
        }
    }
}
