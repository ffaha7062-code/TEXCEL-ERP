using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using TEXCEL_ERP.App_Start;
using TEXCEL_ERP.Data;
using TEXCEL_ERP.Helpers;
using TEXCEL_ERP.Models.Identity;

namespace TEXCEL_ERP
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            System.Data.Entity.Database.SetInitializer(
                new System.Data.Entity.MigrateDatabaseToLatestVersion<ApplicationDbContext, Migrations.Configuration>());

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            UnityConfig.RegisterComponents();
        }

        // ✅ YEH — Har request par chalta hai
        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"[{DateTime.Now}] AcquireRequestState — Authenticated: {HttpContext.Current.User?.Identity?.IsAuthenticated}, Session UserId: {SessionHelper.UserId}");

            // Sirf agar Session available hai
            if (HttpContext.Current.Session == null) return;

            // Agar user authenticated hai
            if (HttpContext.Current.User != null
                && HttpContext.Current.User.Identity.IsAuthenticated)
            {
                // Agar session khali hai — restore karein
                if (SessionHelper.UserId == null)
                {
                    try
                    {
                        var userId = HttpContext.Current.User.Identity.GetUserId();
                        if (!string.IsNullOrEmpty(userId))
                        {
                            var userManager = HttpContext.Current
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
        }
    }
}