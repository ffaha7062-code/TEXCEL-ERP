using System.Web.Mvc;
using TEXCEL_ERP.Helpers;

namespace TEXCEL_ERP.Filters
{
    /// <summary>
    /// Branch-based authorization filter.
    /// </summary>
    public class BranchAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!SessionHelper.UserId.HasValue)
            {
                filterContext.Result = new RedirectResult("~/Account/Login");
                return;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}