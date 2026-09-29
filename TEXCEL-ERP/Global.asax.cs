using System.Data.Entity;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TEXCEL_ERP.App_Start;
using TEXCEL_ERP.Data;
using TEXCEL_ERP.Migrations;

namespace TEXCEL_ERP
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // Database Initializer — Migrate to latest
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, TEXCEL_ERP.Migrations.Configuration>());

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            UnityConfig.RegisterComponents();
        }
    }
}