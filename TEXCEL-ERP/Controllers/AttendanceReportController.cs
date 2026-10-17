using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TEXCEL_ERP.Data;
using TEXCEL_ERP.Data.Repository;
using TEXCEL_ERP.Filters;
using TEXCEL_ERP.Models.Entities;
using TEXCEL_ERP.Models.Enums;
using TEXCEL_ERP.Services;

namespace TEXCEL_ERP.Controllers
{
    /// <summary>
    /// Attendance Reports Controller.
    /// SuperAdmin = all branches, BranchAdmin/Manager = own branch.
    /// Employee = no access.
    /// </summary>
    [RoleAuthorize("SuperAdmin", "BranchAdmin", "Manager")]
    [BranchAuthorize]
    public class AttendanceReportController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IAttendanceReportService _reportService;

        public AttendanceReportController()
        {
            _context = new ApplicationDbContext();
            var uow = new UnitOfWork(_context);
            _reportService = new AttendanceReportService(uow);
        }

        // =============================================
        // INDEX — Reports Hub (Landing Page)
        // =============================================
        // GET: /AttendanceReport
        public ActionResult Index()
        {
            ViewBag.CanModify = CanModify;
            ViewBag.BranchName = IsSuperAdmin ? "All Branches" : CurrentBranchName;
            ViewBag.IsSuperAdmin = IsSuperAdmin;

            // Quick counts for hub cards
            var today = DateTime.Now.Date;
            int? branchId = IsSuperAdmin ? (int?)null : CurrentBranchId;

            var empQuery = _context.Employees.Where(e => e.IsActive);
            if (branchId.HasValue)
            {
                int bid = branchId.Value;
                empQuery = empQuery.Where(e => e.BranchId == bid);
            }
            var empIds = empQuery.Select(e => e.EmployeeId).ToList();

            ViewBag.TotalEmployees = empIds.Count;
            ViewBag.TodayAttendance = _context.Attendances
                .Count(a => a.AttendanceDate == today && empIds.Contains(a.EmployeeId));
            ViewBag.MonthAttendance = _context.Attendances
                .Count(a => a.AttendanceDate.Month == today.Month
                    && a.AttendanceDate.Year == today.Year
                    && empIds.Contains(a.EmployeeId));

            return View();
        }

        // =============================================
        // 1. DAILY REPORT
        // =============================================
        // GET: /AttendanceReport/Daily
        public ActionResult Daily(DateTime? date, int? branchId, int? departmentId)
        {
            var reportDate = (date ?? DateTime.Now).Date;
            int? selectedBranch = IsSuperAdmin ? branchId : CurrentBranchId;

            if (!IsSuperAdmin && !selectedBranch.HasValue)
            {
                TempData["Error"] = "No branch assigned to your account.";
                return RedirectToAction("Index");
            }

            var model = _reportService.GetDailyReport(reportDate, selectedBranch, departmentId);

            ViewBag.CanModify = CanModify;
            ViewBag.IsSuperAdmin = IsSuperAdmin;
            ViewBag.SelectedBranchId = selectedBranch;
            ViewBag.SelectedDepartmentId = departmentId;

            return View(model);
        }

        // =============================================
        // 2. MONTHLY SUMMARY REPORT
        // =============================================
        // GET: /AttendanceReport/Monthly
        public ActionResult Monthly(int? month, int? year, int? branchId, int? departmentId)
        {
            int selMonth = month ?? DateTime.Now.Month;
            int selYear = year ?? DateTime.Now.Year;
            int? selectedBranch = IsSuperAdmin ? branchId : CurrentBranchId;

            if (!IsSuperAdmin && !selectedBranch.HasValue)
            {
                TempData["Error"] = "No branch assigned to your account.";
                return RedirectToAction("Index");
            }

            var model = _reportService.GetMonthlySummaryReport(selMonth, selYear, selectedBranch, departmentId);

            ViewBag.CanModify = CanModify;
            ViewBag.IsSuperAdmin = IsSuperAdmin;
            ViewBag.SelectedBranchId = selectedBranch;
            ViewBag.SelectedDepartmentId = departmentId;

            return View(model);
        }

        // =============================================
        // 3. LATE & OT REPORT
        // =============================================
        // GET: /AttendanceReport/LateOT
        public ActionResult LateOT(DateTime? fromDate, DateTime? toDate, int? branchId, int? departmentId)
        {
            var from = (fromDate ?? DateTime.Now.AddDays(-30)).Date;
            var to = (toDate ?? DateTime.Now).Date;
            int? selectedBranch = IsSuperAdmin ? branchId : CurrentBranchId;

            if (!IsSuperAdmin && !selectedBranch.HasValue)
            {
                TempData["Error"] = "No branch assigned to your account.";
                return RedirectToAction("Index");
            }

            var model = _reportService.GetLateOTReport(from, to, selectedBranch, departmentId);

            ViewBag.CanModify = CanModify;
            ViewBag.IsSuperAdmin = IsSuperAdmin;
            ViewBag.SelectedBranchId = selectedBranch;
            ViewBag.SelectedDepartmentId = departmentId;

            return View(model);
        }

        // =============================================
        // 4. DEPARTMENT-WISE REPORT
        // =============================================
        // GET: /AttendanceReport/Department
        public ActionResult Department(DateTime? fromDate, DateTime? toDate, int? branchId)
        {
            var from = (fromDate ?? DateTime.Now.AddDays(-30)).Date;
            var to = (toDate ?? DateTime.Now).Date;
            int? selectedBranch = IsSuperAdmin ? branchId : CurrentBranchId;

            if (!IsSuperAdmin && !selectedBranch.HasValue)
            {
                TempData["Error"] = "No branch assigned to your account.";
                return RedirectToAction("Index");
            }

            var model = _reportService.GetDepartmentReport(from, to, selectedBranch);

            ViewBag.CanModify = CanModify;
            ViewBag.IsSuperAdmin = IsSuperAdmin;
            ViewBag.SelectedBranchId = selectedBranch;

            return View(model);
        }

        // =============================================
        // 5. GRAPH ANALYTICS
        // =============================================
        // GET: /AttendanceReport/Graphs
        public ActionResult Graphs(int? month, int? year, int? branchId)
        {
            int selMonth = month ?? DateTime.Now.Month;
            int selYear = year ?? DateTime.Now.Year;
            int? selectedBranch = IsSuperAdmin ? branchId : CurrentBranchId;

            if (!IsSuperAdmin && !selectedBranch.HasValue)
            {
                TempData["Error"] = "No branch assigned to your account.";
                return RedirectToAction("Index");
            }

            var model = _reportService.GetGraphAnalytics(selMonth, selYear, selectedBranch);

            ViewBag.CanModify = CanModify;
            ViewBag.IsSuperAdmin = IsSuperAdmin;
            ViewBag.SelectedBranchId = selectedBranch;

            return View(model);
        }

        // =============================================
        // 6. FULL ATTENDANCE REPORT
        // =============================================
        // GET: /AttendanceReport/FullAttendance
        public ActionResult FullAttendance(
            DateTime? fromDate,
            DateTime? toDate,
            int? branchId,
            int? departmentId,
            int? employeeId,
            AttendanceStatus? status)
        {
            var from = (fromDate ?? DateTime.Now.AddDays(-30)).Date;
            var to = (toDate ?? DateTime.Now).Date;
            int? selectedBranch = IsSuperAdmin ? branchId : CurrentBranchId;

            if (!IsSuperAdmin && !selectedBranch.HasValue)
            {
                TempData["Error"] = "No branch assigned to your account.";
                return RedirectToAction("Index");
            }

            var model = _reportService.GetFullAttendanceReport(
                from, to, selectedBranch, departmentId, employeeId, status);

            ViewBag.CanModify = CanModify;
            ViewBag.IsSuperAdmin = IsSuperAdmin;
            ViewBag.SelectedBranchId = selectedBranch;
            ViewBag.SelectedDepartmentId = departmentId;
            ViewBag.SelectedEmployeeId = employeeId;
            ViewBag.SelectedStatus = status;

            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) _context.Dispose();
            base.Dispose(disposing);
        }
    }
}