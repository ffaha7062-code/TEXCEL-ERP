using System;
using System.Web.Mvc;
using TEXCEL_ERP.Helpers;

namespace TEXCEL_ERP.Filters
{
    /// <summary>
    /// Global session timeout filter.
    /// - Skips Account controller and [AllowAnonymous] actions
    /// - Checks authentication (via SessionHelper)
    /// - Checks idle timeout (30 minutes)
    /// - AJAX-aware: returns JSON for AJAX requests, redirect for normal requests
    /// </summary>
    public class SessionTimeoutAttribute : ActionFilterAttribute
    {
        // =============================================
        // IDLE TIMEOUT (MINUTES)
        // =============================================
        private const int TIMEOUT_MINUTES = 1;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            System.Diagnostics.Debug.WriteLine("=== FILTER CHALA: " +
     filterContext.RouteData.Values["controller"] + "/" +
     filterContext.RouteData.Values["action"] +
     " | Auth: " + SessionHelper.IsAuthenticated +
     " | LastAct: " + (filterContext.HttpContext.Session["LastActivity"] ?? "NULL"));

           



            var httpContext = filterContext.HttpContext;

            // =============================================
            // 1. SKIP FOR ACCOUNT CONTROLLER
            // =============================================
            var controllerName = filterContext.RouteData.Values["controller"]?.ToString();
            if (string.Equals(controllerName, "Account", StringComparison.OrdinalIgnoreCase))
            {
                base.OnActionExecuting(filterContext);
                return;
            }

            // =============================================
            // 2. SKIP IF ACTION ALLOWS ANONYMOUS
            // =============================================
            var actionDescriptor = filterContext.ActionDescriptor;

            if (actionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true) ||
                actionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
            {
                base.OnActionExecuting(filterContext);
                return;
            }

            // =============================================
            // 3. AUTHENTICATION CHECK
            // =============================================
            if (!SessionHelper.IsAuthenticated)
            {
                HandleUnauthorized(filterContext);
                return;
            }

            // =============================================
            // 4. IDLE TIMEOUT CHECK
            // =============================================
            var session = httpContext.Session;
            var lastActivity = session["LastActivity"] as DateTime?;

            if (lastActivity.HasValue)
            {
                var idleTime = DateTime.Now - lastActivity.Value;
                if (idleTime.TotalMinutes > TIMEOUT_MINUTES)
                {
                    // Session expired — clear and redirect
                    SessionHelper.Clear();
                    HandleTimeout(filterContext);
                    return;
                }
            }

            // =============================================
            // 5. UPDATE LAST ACTIVITY
            // =============================================
            session["LastActivity"] = DateTime.Now;

            base.OnActionExecuting(filterContext);
        }

        // =============================================
        // HELPERS
        // =============================================
        private void HandleUnauthorized(ActionExecutingContext filterContext)
        {
            if (IsAjaxRequest(filterContext))
            {
                filterContext.Result = new JsonResult
                {
                    Data = new
                    {
                        success = false,
                        message = "Session expired. Please login again.",
                        redirect = "/Account/Login"
                    },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else
            {
                var returnUrl = filterContext.HttpContext.Request.RawUrl;
                filterContext.Result = new RedirectResult(
                    $"~/Account/Login?returnUrl={System.Web.HttpUtility.UrlEncode(returnUrl)}");
            }
        }

        private void HandleTimeout(ActionExecutingContext filterContext)
        {
            if (IsAjaxRequest(filterContext))
            {
                filterContext.Result = new JsonResult
                {
                    Data = new
                    {
                        success = false,
                        message = "Session timed out due to inactivity. Please login again.",
                        redirect = "/Account/Login?timeout=true"
                    },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else
            {
                filterContext.Result = new RedirectResult("~/Account/Login?timeout=true");
            }
        }

        private bool IsAjaxRequest(ActionExecutingContext filterContext)
        {
            var request = filterContext.HttpContext.Request;
            return string.Equals(request.Headers["X-Requested-With"], "XMLHttpRequest",
                       StringComparison.OrdinalIgnoreCase)
                || (request.ContentType != null
                    && request.ContentType.IndexOf("application/json", StringComparison.OrdinalIgnoreCase) >= 0);
        }
    }
}