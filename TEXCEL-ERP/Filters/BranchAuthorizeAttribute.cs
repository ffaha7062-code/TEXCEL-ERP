using System.Web.Mvc;
using TEXCEL_ERP.Helpers;

namespace TEXCEL_ERP.Filters
{
    /// <summary>
    /// Branch-based authorization filter.
    /// Ensures the user is authenticated and has a valid branch.
    /// </summary>
    public class BranchAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // =============================================
            // 1. Check Authentication
            // =============================================
            if (!SessionHelper.IsAuthenticated)
            {
                var returnUrl = filterContext.HttpContext.Request.RawUrl;
                filterContext.Result = new RedirectResult(
                    $"~/Account/Login?returnUrl={System.Web.HttpUtility.UrlEncode(returnUrl)}");
                return;
            }

            // =============================================
            // 2. Check Branch Assignment
            // =============================================
            // SuperAdmin doesn't require BranchId
            if (!SessionHelper.IsSuperAdmin && !SessionHelper.BranchId.HasValue)
            {
                // If non-admin has no branch → logout or error
                filterContext.Result = new RedirectResult("~/Home/Unauthorized");
                return;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}