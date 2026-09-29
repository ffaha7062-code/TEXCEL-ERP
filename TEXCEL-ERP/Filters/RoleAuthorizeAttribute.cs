using System;
using System.Web.Mvc;
using TEXCEL_ERP.Helpers;

namespace TEXCEL_ERP.Filters
{
    /// <summary>
    /// Role-based authorization filter.
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
            if (!SessionHelper.UserId.HasValue)
            {
                filterContext.Result = new RedirectResult("~/Account/Login");
                return;
            }

            var currentRole = SessionHelper.Role;
            if (Array.IndexOf(_allowedRoles, currentRole) < 0 && currentRole != "SuperAdmin")
            {
                filterContext.Result = new RedirectResult("~/Home/Unauthorized");
                return;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}