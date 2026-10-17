using System;
using System.Web.Mvc;
using TEXCEL_ERP.Helpers;

namespace TEXCEL_ERP.Filters
{
    /// <summary>
    /// Role-based authorization filter.
    /// Usage: [RoleAuthorize("SuperAdmin", "Manager")]
    /// </summary>
    public class RoleAuthorizeAttribute : ActionFilterAttribute
    {
        private readonly string[] _allowedRoles;

        public RoleAuthorizeAttribute(params string[] allowedRoles)
        {
            _allowedRoles = allowedRoles;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Check authentication
            if (!SessionHelper.IsAuthenticated)
            {
                var returnUrl = filterContext.HttpContext.Request.RawUrl;
                filterContext.Result = new RedirectResult(
                    $"~/Account/Login?returnUrl={System.Web.HttpUtility.UrlEncode(returnUrl)}");
                return;
            }

            // SuperAdmin always allowed
            if (SessionHelper.IsSuperAdmin)
            {
                base.OnActionExecuting(filterContext);
                return;
            }

            // Check role
            var currentRole = SessionHelper.Role;
            if (Array.IndexOf(_allowedRoles, currentRole) < 0)
            {
                filterContext.Result = new RedirectResult("~/Home/Unauthorized");
                return;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}