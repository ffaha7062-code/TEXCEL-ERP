using System;
using System.Web.Mvc;
using TEXCEL_ERP.Data;
using TEXCEL_ERP.Data.Repository;
using TEXCEL_ERP.Filters;
using TEXCEL_ERP.Helpers;
using TEXCEL_ERP.Services;

namespace TEXCEL_ERP.Controllers
{
    /// <summary>
    /// Dashboard controller.
    /// SuperAdmin = all branches data
    /// BranchAdmin/Manager/Employee = own branch data
    /// </summary>
    [RoleAuthorize("SuperAdmin", "BranchAdmin", "Manager", "Employee")]
    [BranchAuthorize]
    public class DashboardController : BaseController
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController()
        {
            var context = new ApplicationDbContext();
            var unitOfWork = new UnitOfWork(context);
            _dashboardService = new DashboardService(unitOfWork);
        }

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        // =============================================
        // INDEX — Dashboard
        // =============================================
        // GET: /Dashboard/Index
        public ActionResult Index()
        {
            // ✅ Role-based data fetch
            var model = GetRoleBasedDashboardData();

            // ✅ User info from SessionHelper (consistent)
            model.UserName = CurrentFullName;
            model.Role = CurrentRole;
            model.BranchName = IsSuperAdmin ? "All Branches" : CurrentBranchName;

            return View(model);
        }

        // =============================================
        // GET CHART DATA — AJAX
        // =============================================
        [HttpGet]
        public JsonResult GetChartData()
        {
            var model = GetRoleBasedDashboardData();

            return Json(new
            {
                sales = model.MonthlySalesChart,
                production = model.MonthlyProductionChart,
                purchase = model.MonthlyPurchaseChart
            }, JsonRequestBehavior.AllowGet);
        }

        // =============================================
        // HELPER — Role-based data fetch
        // =============================================
        private Models.ViewModels.DashboardViewModel GetRoleBasedDashboardData()
        {
            // SuperAdmin = all branches
            if (IsSuperAdmin)
            {
                return _dashboardService.GetDashboardData();
            }

            // BranchAdmin/Manager/Employee = own branch
            if (CurrentBranchId.HasValue)
            {
                return _dashboardService.GetBranchDashboardData(CurrentBranchId.Value);
            }

            // Fallback: no branch — return all data
            return _dashboardService.GetDashboardData();
        }
    }
}