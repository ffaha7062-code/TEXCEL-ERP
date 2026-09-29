using System.Web.Mvc;
using TEXCEL_ERP.Filters;
using TEXCEL_ERP.Services;
using TEXCEL_ERP.Helpers;
using Microsoft.AspNet.Identity;

namespace TEXCEL_ERP.Controllers
{
    /// <summary>
    /// Dashboard controller for the application.
    /// </summary>
    [SessionTimeout]
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController()
        {
            var context = new Data.ApplicationDbContext();
            var unitOfWork = new Data.Repository.UnitOfWork(context);
            _dashboardService = new DashboardService(unitOfWork);
        }

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        // GET: /Dashboard/Index
        public ActionResult Index()
        {
            var model = _dashboardService.GetDashboardData();
            model.UserName = User.Identity.GetUserName() ?? "User";
            model.Role = SessionHelper.Role ?? "User";
            model.BranchName = SessionHelper.BranchName ?? "Head Office";

            return View(model);
        }

        // GET: /Dashboard/GetChartData (AJAX)
        [HttpGet]
        public JsonResult GetChartData()
        {
            var model = _dashboardService.GetDashboardData();
            return Json(new
            {
                sales = model.MonthlySalesChart,
                production = model.MonthlyProductionChart,
                purchase = model.MonthlyPurchaseChart
            }, JsonRequestBehavior.AllowGet);
        }
    }
}