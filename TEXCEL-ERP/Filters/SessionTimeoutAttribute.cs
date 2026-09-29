using System.Web.Mvc;

namespace TEXCEL_ERP.Filters
{
    /// <summary>
    /// Session timeout filter.
    /// Redirects unauthenticated users to login page.
    /// Skips Account controller and AllowAnonymous actions.
    /// </summary>
    public class SessionTimeoutAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var httpContext = filterContext.HttpContext;

            // =============================================
            // SKIP FOR ACCOUNT CONTROLLER
            // =============================================
            var controllerName = filterContext.RouteData.Values["controller"]?.ToString();
            if (controllerName == "Account")
            {
                base.OnActionExecuting(filterContext);
                return;
            }

            // =============================================
            // SKIP IF ACTION ALLOWS ANONYMOUS
            // =============================================
            var actionName = filterContext.RouteData.Values["action"]?.ToString();
            var actionDescriptor = filterContext.ActionDescriptor;

            if (actionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true) ||
                actionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
            {
                base.OnActionExecuting(filterContext);
                return;
            }

            // =============================================
            // CHECK AUTHENTICATION
            // =============================================
            if (!httpContext.User.Identity.IsAuthenticated)
            {
                var returnUrl = httpContext.Request.RawUrl;
                filterContext.Result = new RedirectResult($"~/Account/Login?returnUrl={System.Web.HttpUtility.UrlEncode(returnUrl)}");
                return;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}