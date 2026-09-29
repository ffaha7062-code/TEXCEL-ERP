using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace TEXCEL_ERP.Filters
{
    /// <summary>
    /// Exception logging filter.
    /// </summary>
    public class ExceptionLogAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            try
            {
                var logPath = HttpContext.Current.Server.MapPath("~/App_Data/Logs/error_log.txt");
                var directory = Path.GetDirectoryName(logPath);
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                var logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] " +
                    $"Controller: {filterContext.RouteData.Values["controller"]}, " +
                    $"Action: {filterContext.RouteData.Values["action"]}, " +
                    $"Error: {filterContext.Exception.Message}\n" +
                    $"Stack Trace: {filterContext.Exception.StackTrace}\n" +
                    new string('-', 80) + "\n";

                File.AppendAllText(logPath, logMessage);

                filterContext.ExceptionHandled = true;
                filterContext.Result = new ViewResult
                {
                    ViewName = "~/Views/Shared/Error.cshtml",
                    ViewData = new ViewDataDictionary(filterContext.Controller.ViewData)
                    {
                        Model = new HandleErrorInfo(filterContext.Exception,
                            filterContext.RouteData.Values["controller"].ToString(),
                            filterContext.RouteData.Values["action"].ToString())
                    }
                };
            }
            catch { }
        }
    }
}