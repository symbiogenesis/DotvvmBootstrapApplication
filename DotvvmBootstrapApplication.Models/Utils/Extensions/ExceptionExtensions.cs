using System;

namespace DotvvmBootstrapApplication.Utils.Extensions
{
    public static class ExceptionExtensions
    {
        public static string GetErrorMessage(this Exception ex)
        {
            //switch (ex.Message)
            //{
            //    case "Illegal command invocation!":
            //        return "Illegal command invocation!";
            //}

            return "Something went wrong";
        }
    }
}
