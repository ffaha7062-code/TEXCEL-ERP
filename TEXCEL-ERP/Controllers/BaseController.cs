using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TEXCEL_ERP.Data;
using TEXCEL_ERP.Helpers;
using TEXCEL_ERP.Models.Identity;

using Microsoft.AspNet.Identity.Owin;

namespace TEXCEL_ERP.Controllers
{
    /// <summary>
    /// BaseController — Sab controllers isse inherit karenge.
    /// Branch isolation + Role helpers yahan centralized hain.
    /// Session check ab Global Filter (SessionTimeoutAttribute) kar raha hai.
    /// </summary>
    public class BaseController : Controller
    {
        // ==================== CURRENT USER INFO ====================

        protected string CurrentUserId
        {
            get { return SessionHelper.UserId ?? string.Empty; }
        }

        protected int CurrentUserIdInt
        {
            get
            {
                int id;
                if (int.TryParse(SessionHelper.UserId, out id)) return id;
                return 0;
            }
        }

        protected string CurrentUserName
        {
            get { return SessionHelper.UserName ?? "Guest"; }
        }

        protected string CurrentFullName
        {
            get { return SessionHelper.FullName ?? "Guest"; }
        }

        protected string CurrentRole
        {
            get { return SessionHelper.Role ?? "Employee"; }
        }

        protected int? CurrentBranchId
        {
            get { return SessionHelper.BranchId; }
        }

        protected string CurrentBranchName
        {
            get { return SessionHelper.BranchName ?? "All Branches"; }
        }

        protected int? CurrentEmployeeId
        {
            get { return SessionHelper.EmployeeId; }
        }

        // ==================== ROLE HELPERS ====================

        protected bool IsSuperAdmin
        {
            get { return SessionHelper.IsSuperAdmin; }
        }

        protected bool IsBranchAdmin
        {
            get { return SessionHelper.IsBranchAdmin; }
        }

        protected bool IsManager
        {
            get { return SessionHelper.IsManager; }
        }

        protected bool IsEmployee
        {
            get { return SessionHelper.IsEmployee; }
        }

        protected bool CanModify
        {
            get { return SessionHelper.CanEdit; }
        }

        protected bool CanDeleteRecord
        {
            get { return SessionHelper.CanDelete; }
        }

        // ==================== BRANCH ISOLATION ====================

        protected IQueryable<T> ApplyBranchFilter<T>(IQueryable<T> query,
            Func<T, int?> branchSelector)
        {
            if (IsSuperAdmin) return query;

            if (!CurrentBranchId.HasValue)
                return query.Where(x => false);

            int branchId = CurrentBranchId.Value;
            return query.Where(x => branchSelector(x) == branchId);
        }

        protected bool CanAccessBranch(int branchId)
        {
            if (IsSuperAdmin) return true;
            return CurrentBranchId.HasValue && CurrentBranchId.Value == branchId;
        }

        protected bool CanAccessRecord(int? recordBranchId)
        {
            if (IsSuperAdmin) return true;
            if (IsEmployee) return false;
            if (!recordBranchId.HasValue) return false;
            return CurrentBranchId.HasValue && CurrentBranchId.Value == recordBranchId.Value;
        }

        // ==================== JSON RESPONSE HELPERS ====================

        protected JsonResult SuccessResult(string message, object data = null)
        {
            return Json(new { success = true, message = message, data = data },
                JsonRequestBehavior.AllowGet);
        }

        protected JsonResult ErrorResult(string message)
        {
            return Json(new { success = false, message = message },
                JsonRequestBehavior.AllowGet);
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // ✅ Session restore — agar khali hai
            if (!SessionHelper.IsAuthenticated)
            {
                // OWIN se user check karein
                if (filterContext.HttpContext.User != null
                    && filterContext.HttpContext.User.Identity.IsAuthenticated)
                {
                    try
                    {
                        var userId = filterContext.HttpContext.User.Identity.GetUserId();
                        if (!string.IsNullOrEmpty(userId))
                        {
                            var userManager = filterContext.HttpContext
                                .GetOwinContext()
                                .GetUserManager<ApplicationUserManager>();

                            var user = userManager.FindById(userId);
                            if (user != null)
                            {
                                var roles = userManager.GetRoles(userId);
                                var primaryRole = roles.FirstOrDefault() ?? "Employee";

                                string branchName = null;
                                if (user.BranchId.HasValue)
                                {
                                    using (var context = new ApplicationDbContext())
                                    {
                                        var branch = context.Branches.Find(user.BranchId.Value);
                                        if (branch != null) branchName = branch.BranchName;
                                    }
                                }

                                SessionHelper.UserId = user.Id;
                                SessionHelper.UserName = user.UserName;
                                SessionHelper.FullName = user.FullName;
                                SessionHelper.Role = primaryRole;
                                SessionHelper.BranchId = user.BranchId;
                                SessionHelper.BranchName = branchName;
                                SessionHelper.EmployeeId = user.EmployeeId;
                            }
                        }
                    }
                    catch { /* Silent */ }
                }
            }

            // Authentication check
            if (!SessionHelper.IsAuthenticated)
            {
                var returnUrl = filterContext.HttpContext.Request.RawUrl;
                filterContext.Result = new RedirectResult(
                    $"~/Account/Login?returnUrl={System.Web.HttpUtility.UrlEncode(returnUrl)}");
                return;
            }

            base.OnActionExecuting(filterContext);
        }


    }

}