using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TEXCEL_ERP.Data;
using TEXCEL_ERP.Data.Repository;
using TEXCEL_ERP.Filters;
using TEXCEL_ERP.Models.Entities;
using TEXCEL_ERP.Models.Enums;
using TEXCEL_ERP.Models.ViewModels;
using TEXCEL_ERP.Services;

namespace TEXCEL_ERP.Controllers
{
    /// <summary>
    /// Attendance controller.
    /// SuperAdmin = all branches, BranchAdmin/Manager = own branch.
    /// Employee = sirf apni attendance.
    /// </summary>
    [RoleAuthorize("SuperAdmin", "BranchAdmin", "Manager", "Employee")]
    [BranchAuthorize]
    public class AttendanceController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IAttendanceService _attendanceService;
        private readonly IHolidayService _holidayService;

        public AttendanceController()
        {
            _context = new ApplicationDbContext();
            var uow = new UnitOfWork(_context);
            _attendanceService = new AttendanceService(uow);
            _holidayService = new HolidayService(uow);
        }

        // =============================================
        // INDEX — Attendance Landing
        // =============================================
        // GET: /Attendance
        // =============================================
        // INDEX — Attendance Landing Page
        // =============================================
        public ActionResult Index()
        {
            // ✅ Employee → My Attendance pe redirect
            if (IsEmployee && CurrentEmployeeId.HasValue)
            {
                return RedirectToAction("MyAttendance");
            }

            // ✅ SuperAdmin / BranchAdmin / Manager → Landing page
            ViewBag.CanModify = CanModify;
            ViewBag.CanDelete = CanDeleteRecord;

            // Quick stats for landing page
            var today = DateTime.Now.Date;
            int? branchId = IsSuperAdmin ? (int?)null : CurrentBranchId;

            var query = _context.Attendances.Where(a => a.AttendanceDate == today);
            if (branchId.HasValue)
            {
                int bid = branchId.Value;
                query = query.Where(a => a.BranchId == bid);
            }

            ViewBag.TodayPresent = query.Count(a => a.Status == AttendanceStatus.Present);
            ViewBag.TodayAbsent = query.Count(a => a.Status == AttendanceStatus.Absent);
            ViewBag.TodayLeave = query.Count(a => a.Status == AttendanceStatus.Leave);
            ViewBag.TodayTotal = query.Count();

            // Upcoming holidays count
            var holidaysQuery = _context.Holidays.Where(h => h.HolidayDate >= today && h.IsActive);
            if (branchId.HasValue)
            {
                int bid = branchId.Value;
                holidaysQuery = holidaysQuery.Where(h => h.BranchId == bid);
            }
            ViewBag.UpcomingHolidays = holidaysQuery.Count();

            return View();
        }

        // =============================================
        // BULK ENTRY — Main Page (Notepad Format)
        // =============================================
        // GET: /Attendance/BulkEntry
        public ActionResult BulkEntry(DateTime? date, int? branchId, int? shiftId)
        {
            if (IsEmployee)
            {
                return RedirectToAction("MyAttendance");
            }

            var attendanceDate = (date ?? DateTime.Now).Date;

            // ✅ Branch isolation
            int? selectedBranch = IsSuperAdmin ? branchId : CurrentBranchId;
            if (!selectedBranch.HasValue && !IsSuperAdmin)
            {
                TempData["Error"] = "No branch assigned to your account.";
                return RedirectToAction("Index", "Dashboard");
            }

            // Branch info
            ViewBag.AttendanceDate = attendanceDate;
            ViewBag.SelectedBranchId = selectedBranch;
            ViewBag.SelectedShiftId = shiftId;
            ViewBag.CanModify = CanModify;
            ViewBag.CanDelete = CanDeleteRecord;

            // Branches dropdown
            if (IsSuperAdmin)
            {
                ViewBag.Branches = _context.Branches.Where(b => b.IsActive).ToList();
            }
            else if (CurrentBranchId.HasValue)
            {
                int bid = CurrentBranchId.Value;
                ViewBag.Branches = _context.Branches.Where(b => b.IsActive && b.BranchId == bid).ToList();
            }
            else
            {
                ViewBag.Branches = new List<Branch>();
            }

            // Shifts dropdown (branch-specific)
            if (selectedBranch.HasValue)
            {
                int sb = selectedBranch.Value;
                ViewBag.Shifts = _context.Shifts
                    .Where(s => s.IsActive && s.BranchId == sb)
                    .ToList();
            }
            else
            {
                ViewBag.Shifts = _context.Shifts.Where(s => s.IsActive).ToList();
            }

            // ✅ Employee list with attendance data
            var employeeList = new List<EmployeeAttendanceRow>();

            if (selectedBranch.HasValue)
            {
                int sb = selectedBranch.Value;

                var employeesQuery = _context.Employees
                    .Where(e => e.BranchId == sb && e.IsActive);

                if (shiftId.HasValue)
                {
                    employeesQuery = employeesQuery.Where(e => e.ShiftId == shiftId.Value);
                }

                var employees = employeesQuery.OrderBy(e => e.FirstName).ToList();

                var employeeIds = employees.Select(e => e.EmployeeId).ToList();

                // Existing attendance
                var existingAttendances = _context.Attendances
                    .Where(a => a.AttendanceDate == attendanceDate
                        && employeeIds.Contains(a.EmployeeId))
                    .ToList();

                foreach (var emp in employees)
                {
                    var att = existingAttendances.FirstOrDefault(a => a.EmployeeId == emp.EmployeeId);

                    employeeList.Add(new EmployeeAttendanceRow
                    {
                        EmployeeId = emp.EmployeeId,
                        EmployeeCode = emp.EmployeeCode,
                        EmployeeName = emp.FullName,
                        ShiftId = emp.ShiftId,
                        ShiftName = emp.Shift != null ? emp.Shift.ShiftName : "-",
                        ShiftStartTime = emp.Shift != null ? emp.Shift.StartTime : TimeSpan.Zero,
                        ShiftEndTime = emp.Shift != null ? emp.Shift.EndTime : TimeSpan.Zero,
                        BreakDuration = emp.Shift != null ? emp.Shift.BreakDuration : 0,
                        AttendanceId = att != null ? att.AttendanceId : 0,
                        CheckIn = att != null ? att.CheckIn : null,
                        CheckOut = att != null ? att.CheckOut : null,
                        Status = att != null ? att.Status : AttendanceStatus.Absent,
                        WorkedHours = att != null ? att.WorkedHours : 0,
                        OvertimeHours = att != null ? att.OvertimeHours : 0,
                        LessTimeHours = att != null ? att.LessTimeHours : 0,
                        LateMinutes = att != null ? att.LateMinutes : 0,
                        EarlyGoingMinutes = att != null ? att.EarlyGoingMinutes : 0,
                        Remarks = att != null ? att.Remarks : null,
                        IsVerified = att != null && att.IsVerified,
                        HasAttendance = att != null
                    });
                }
            }

            return View(employeeList);
        }

        // =============================================
        // SAVE BULK — AJAX POST
        // =============================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveBulk(List<BulkAttendanceInput> model, DateTime attendanceDate)
        {
            try
            {
                if (!CanModify)
                {
                    return ErrorResult("You don't have permission to mark attendance.");
                }

                if (model == null || !model.Any())
                {
                    return ErrorResult("No attendance data provided.");
                }

                int? selectedBranch = IsSuperAdmin ? null : CurrentBranchId;

                var attendanceList = new List<Attendance>();
               
                foreach (var item in model)
                {
                    // Employee check
                    var employee = _context.Employees.Find(item.EmployeeId);
                    if (employee == null) continue;

                    // ✅ Branch isolation check
                    if (!IsSuperAdmin && CurrentBranchId.HasValue)
                    {
                        if (employee.BranchId != CurrentBranchId.Value)
                        {
                            continue;
                        }
                    }

                    // Parse times
                    TimeSpan? checkIn = null;
                    TimeSpan? checkOut = null;

                    if (!string.IsNullOrWhiteSpace(item.CheckIn))
                    {
                        TimeSpan parsed;
                        if (TimeSpan.TryParse(item.CheckIn, out parsed)) checkIn = parsed;
                    }

                    if (!string.IsNullOrWhiteSpace(item.CheckOut))
                    {
                        TimeSpan parsed;
                        if (TimeSpan.TryParse(item.CheckOut, out parsed)) checkOut = parsed;
                    }

                    var attendance = new Attendance
                    {
                        EmployeeId = item.EmployeeId,
                        BranchId = employee.BranchId,
                        AttendanceDate = attendanceDate.Date,
                        ShiftId = employee.ShiftId,
                        CheckIn = checkIn,
                        CheckOut = checkOut,
                        Status = (AttendanceStatus)item.Status,
                        Remarks = item.Remarks,
                        IsVerified = item.IsVerified,
                        CreatedDate = DateTime.Now
                    };

                    // Calculate hours
                    if (employee.ShiftId.HasValue && checkIn.HasValue && checkOut.HasValue)
                    {
                        var shift = _context.Shifts.Find(employee.ShiftId.Value);
                        if (shift != null)
                        {
                            _attendanceService.CalculateAttendanceHours(attendance, shift);
                        }
                    }

                    attendanceList.Add(attendance);
                }

                _attendanceService.SaveBulkAttendance(attendanceList);

                return SuccessResult($"{attendanceList.Count} attendance records saved successfully!");
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                if (ex.InnerException != null)
                    msg += " | " + ex.InnerException.Message;
                return ErrorResult("Error: " + msg);
            }
        }

        // =============================================
        // GET BY ID — For Edit
        // =============================================
        [HttpGet]
        public JsonResult GetById(int id)
        {
            try
            {
                var att = _context.Attendances.Find(id);
                if (att == null) return ErrorResult("Attendance not found.");

                if (!IsSuperAdmin && CurrentBranchId.HasValue)
                {
                    if (att.BranchId != CurrentBranchId.Value)
                        return ErrorResult("Permission denied.");
                }

                return Json(new
                {
                    success = true,
                    data = new
                    {
                        att.AttendanceId,
                        att.EmployeeId,
                        CheckIn = att.CheckIn.HasValue ? att.CheckIn.Value.ToString(@"hh\:mm") : "",
                        CheckOut = att.CheckOut.HasValue ? att.CheckOut.Value.ToString(@"hh\:mm") : "",
                        Status = (int)att.Status,
                        att.WorkedHours,
                        att.OvertimeHours,
                        att.LessTimeHours,
                        att.LateMinutes,
                        att.EarlyGoingMinutes,
                        att.Remarks,
                        att.IsVerified
                    }
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return ErrorResult("Error: " + ex.Message);
            }
        }

        // =============================================
        // MONTHLY GRID
        // =============================================
        public ActionResult MonthlyGrid(int? month, int? year, int? branchId, int? departmentId)
        {
            if (IsEmployee)
            {
                return RedirectToAction("MyAttendance");
            }

            int selMonth = month ?? DateTime.Now.Month;
            int selYear = year ?? DateTime.Now.Year;
            int? selBranch = IsSuperAdmin ? branchId : CurrentBranchId;

            ViewBag.Month = selMonth;
            ViewBag.Year = selYear;
            ViewBag.SelectedBranchId = selBranch;
            ViewBag.SelectedDepartmentId = departmentId;
            ViewBag.CanModify = CanModify;

            // Branches dropdown
            if (IsSuperAdmin)
            {
                ViewBag.Branches = _context.Branches.Where(b => b.IsActive).ToList();
            }
            else if (CurrentBranchId.HasValue)
            {
                int bid = CurrentBranchId.Value;
                ViewBag.Branches = _context.Branches.Where(b => b.IsActive && b.BranchId == bid).ToList();
            }

            // Departments dropdown
            if (selBranch.HasValue)
            {
                int sb = selBranch.Value;
                ViewBag.Departments = _context.Departments.Where(d => d.IsActive && d.BranchId == sb).ToList();
            }
            else
            {
                ViewBag.Departments = _context.Departments.Where(d => d.IsActive).ToList();
            }

            // Grid data
            var employeesQuery = _context.Employees.Where(e => e.IsActive);

            if (selBranch.HasValue)
            {
                int sb = selBranch.Value;
                employeesQuery = employeesQuery.Where(e => e.BranchId == sb);
            }

            if (departmentId.HasValue)
            {
                int did = departmentId.Value;
                employeesQuery = employeesQuery.Where(e => e.DepartmentId == did);
            }

            var employees = employeesQuery.OrderBy(e => e.FirstName).ToList();
            var employeeIds = employees.Select(e => e.EmployeeId).ToList();

            // Month attendances
            var attendances = _context.Attendances
                .Where(a => a.AttendanceDate.Month == selMonth
                    && a.AttendanceDate.Year == selYear
                    && employeeIds.Contains(a.EmployeeId))
                .ToList();

            // Grid model
            var gridModel = new MonthlyAttendanceGridViewModel
            {
                Month = selMonth,
                Year = selYear,
                MonthName = new DateTime(selYear, selMonth, 1).ToString("MMMM yyyy"),
                Employees = employees.Select(e => new MonthlyGridEmployee
                {
                    EmployeeId = e.EmployeeId,
                    EmployeeCode = e.EmployeeCode,
                    EmployeeName = e.FullName,
                    DepartmentName = e.Department != null ? e.Department.DepartmentName : "-",
                    DesignationName = e.Designation != null ? e.Designation.DesignationName : "-",
                    DailyStatus = GetMonthlyStatus(attendances, e.EmployeeId, selMonth, selYear)
                }).ToList()
            };

            return View(gridModel);
        }

        private Dictionary<int, string> GetMonthlyStatus(List<Attendance> attendances, int employeeId, int month, int year)
        {
            var daysInMonth = DateTime.DaysInMonth(year, month);
            var result = new Dictionary<int, string>();

            for (int day = 1; day <= daysInMonth; day++)
            {
                var att = attendances.FirstOrDefault(a => a.EmployeeId == employeeId && a.AttendanceDate.Day == day);
                if (att == null)
                {
                    result[day] = "-";
                }
                else
                {
                    switch (att.Status)
                    {
                        case AttendanceStatus.Present: result[day] = "P"; break;
                        case AttendanceStatus.Absent: result[day] = "A"; break;
                        case AttendanceStatus.Leave: result[day] = "L"; break;
                        case AttendanceStatus.HalfDay: result[day] = "H"; break;
                        case AttendanceStatus.Holiday: result[day] = "HO"; break;
                        case AttendanceStatus.Weekend: result[day] = "W"; break;
                        default: result[day] = "-"; break;
                    }
                }
            }

            return result;
        }

        // =============================================
        // MY ATTENDANCE — Employee
        // =============================================
        public ActionResult MyAttendance(int? month, int? year, int? employeeId)
        {
            int empId;

            // Employee role → apna profile
            if (CurrentEmployeeId.HasValue)
            {
                empId = CurrentEmployeeId.Value;
            }
            // SuperAdmin/BranchAdmin → employee select kar sakte hain
            else if (employeeId.HasValue)
            {
                empId = employeeId.Value;
            }
            // Agar koi employeeId nahi diya → pehla employee le lein
            else
            {
                var firstEmp = _context.Employees
                    .Where(e => e.IsActive)
                    .OrderBy(e => e.EmployeeId)
                    .FirstOrDefault();

                if (firstEmp == null)
                {
                    TempData["Error"] = "No employees found.";
                    return RedirectToAction("Index", "Dashboard");
                }
                empId = firstEmp.EmployeeId;
            }

            int selMonth = month ?? DateTime.Now.Month;
            int selYear = year ?? DateTime.Now.Year;

            var attendances = _context.Attendances
                .Where(a => a.EmployeeId == empId
                    && a.AttendanceDate.Month == selMonth
                    && a.AttendanceDate.Year == selYear)
                .OrderBy(a => a.AttendanceDate)
                .ToList();

            var employee = _context.Employees.Find(empId);

            ViewBag.Month = selMonth;
            ViewBag.Year = selYear;
            ViewBag.MonthName = new DateTime(selYear, selMonth, 1).ToString("MMMM yyyy");
            ViewBag.Employee = employee;
            ViewBag.TotalPresent = attendances.Count(a => a.Status == AttendanceStatus.Present);
            ViewBag.TotalAbsent = attendances.Count(a => a.Status == AttendanceStatus.Absent);
            ViewBag.TotalLeave = attendances.Count(a => a.Status == AttendanceStatus.Leave);
            ViewBag.TotalLate = attendances.Count(a => a.LateMinutes > 0);
            ViewBag.TotalOvertime = attendances.Sum(a => a.OvertimeHours);
            ViewBag.TotalLessTime = attendances.Sum(a => a.LessTimeHours);

            // ✅ SuperAdmin/Manager → Employee dropdown
            if (!IsEmployee)
            {
                ViewBag.Employees = _context.Employees
                    .Where(e => e.IsActive)
                    .OrderBy(e => e.FirstName)
                    .ToList();
            }

            return View(attendances);
        }


        // =============================================
        // SINGLE ENTRY — GET
        // =============================================
        public ActionResult SingleEntry(int? employeeId, DateTime? date)
        {
            if (IsEmployee)
            {
                return RedirectToAction("MyAttendance");
            }

            var attendanceDate = (date ?? DateTime.Now).Date;

            ViewBag.AttendanceDate = attendanceDate;
            ViewBag.SelectedEmployeeId = employeeId;
            ViewBag.CanModify = CanModify;
            ViewBag.CanDelete = CanDeleteRecord;

            // ✅ Branch isolation — Employees dropdown
            var employeesQuery = _context.Employees.Where(e => e.IsActive);

            if (!IsSuperAdmin && CurrentBranchId.HasValue)
            {
                int bid = CurrentBranchId.Value;
                employeesQuery = employeesQuery.Where(e => e.BranchId == bid);
            }

            ViewBag.Employees = employeesQuery
                .OrderBy(e => e.FirstName)
                .ToList();

            // Employee data (agar select hua)
            EmployeeAttendanceRow row = null;

            if (employeeId.HasValue)
            {
                var emp = _context.Employees.Find(employeeId.Value);

                if (emp != null)
                {
                    // Branch isolation check
                    if (!IsSuperAdmin && CurrentBranchId.HasValue)
                    {
                        if (emp.BranchId != CurrentBranchId.Value)
                        {
                            TempData["Error"] = "Permission denied.";
                            return RedirectToAction("SingleEntry");
                        }
                    }

                    var existing = _context.Attendances
                        .FirstOrDefault(a => a.EmployeeId == emp.EmployeeId && a.AttendanceDate == attendanceDate);

                    var shift = emp.ShiftId.HasValue ? _context.Shifts.Find(emp.ShiftId.Value) : null;

                    row = new EmployeeAttendanceRow
                    {
                        EmployeeId = emp.EmployeeId,
                        EmployeeCode = emp.EmployeeCode,
                        EmployeeName = emp.FullName,
                        ShiftId = emp.ShiftId,
                        ShiftName = shift != null ? shift.ShiftName : "-",
                        ShiftStartTime = shift != null ? shift.StartTime : TimeSpan.Zero,
                        ShiftEndTime = shift != null ? shift.EndTime : TimeSpan.Zero,
                        BreakDuration = shift != null ? shift.BreakDuration : 0,
                        AttendanceId = existing != null ? existing.AttendanceId : 0,
                        CheckIn = existing != null ? existing.CheckIn : null,
                        CheckOut = existing != null ? existing.CheckOut : null,
                        Status = existing != null ? existing.Status : AttendanceStatus.Present,
                        WorkedHours = existing != null ? existing.WorkedHours : 0,
                        OvertimeHours = existing != null ? existing.OvertimeHours : 0,
                        LessTimeHours = existing != null ? existing.LessTimeHours : 0,
                        LateMinutes = existing != null ? existing.LateMinutes : 0,
                        EarlyGoingMinutes = existing != null ? existing.EarlyGoingMinutes : 0,
                        Remarks = existing != null ? existing.Remarks : null,
                        IsVerified = existing != null && existing.IsVerified,
                        HasAttendance = existing != null
                    };
                }
            }

            return View(row);
        }

        // =============================================
        // SINGLE SAVE — AJAX POST
        // =============================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SaveSingle(int employeeId, DateTime attendanceDate, string checkIn, string checkOut, int status, bool isVerified, string remarks)
        {
            try
            {
                if (!CanModify)
                    return ErrorResult("Permission denied.");

                var employee = _context.Employees.Find(employeeId);
                if (employee == null)
                    return ErrorResult("Employee not found.");

                // Branch isolation
                if (!IsSuperAdmin && CurrentBranchId.HasValue)
                {
                    if (employee.BranchId != CurrentBranchId.Value)
                        return ErrorResult("Permission denied.");
                }

                TimeSpan? ci = null, co = null;

                if (!string.IsNullOrWhiteSpace(checkIn))
                {
                    TimeSpan parsed;
                    if (TimeSpan.TryParse(checkIn, out parsed)) ci = parsed;
                }

                if (!string.IsNullOrWhiteSpace(checkOut))
                {
                    TimeSpan parsed;
                    if (TimeSpan.TryParse(checkOut, out parsed)) co = parsed;
                }

                var existing = _context.Attendances
                    .FirstOrDefault(a => a.EmployeeId == employeeId && a.AttendanceDate == attendanceDate.Date);

                if (existing != null)
                {
                    existing.CheckIn = ci;
                    existing.CheckOut = co;
                    existing.Status = (AttendanceStatus)status;
                    existing.IsVerified = isVerified;
                    existing.Remarks = remarks;
                    existing.ModifiedDate = DateTime.Now;

                    // Calculate hours
                    if (employee.ShiftId.HasValue && ci.HasValue && co.HasValue)
                    {
                        var shift = _context.Shifts.Find(employee.ShiftId.Value);
                        if (shift != null)
                        {
                            _attendanceService.CalculateAttendanceHours(existing, shift);
                        }
                    }

                    _context.SaveChanges();
                    return SuccessResult("Attendance updated successfully!");
                }
                else
                {
                    var att = new Attendance
                    {
                        EmployeeId = employeeId,
                        BranchId = employee.BranchId,
                        AttendanceDate = attendanceDate.Date,
                        ShiftId = employee.ShiftId,
                        CheckIn = ci,
                        CheckOut = co,
                        Status = (AttendanceStatus)status,
                        IsVerified = isVerified,
                        Remarks = remarks,
                        CreatedDate = DateTime.Now
                    };

                    if (employee.ShiftId.HasValue && ci.HasValue && co.HasValue)
                    {
                        var shift = _context.Shifts.Find(employee.ShiftId.Value);
                        if (shift != null)
                        {
                            _attendanceService.CalculateAttendanceHours(att, shift);
                        }
                    }

                    _context.Attendances.Add(att);
                    _context.SaveChanges();
                    return SuccessResult("Attendance marked successfully!");
                }
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                if (ex.InnerException != null) msg += " | " + ex.InnerException.Message;
                return ErrorResult("Error: " + msg);
            }
        }

        // =============================================
        // DELETE — AJAX
        // =============================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Delete(int id)
        {
            try
            {
                if (!CanDeleteRecord)
                {
                    return ErrorResult("You don't have permission to delete attendance.");
                }

                var att = _context.Attendances.Find(id);
                if (att == null) return ErrorResult("Attendance not found.");

                if (!IsSuperAdmin && CurrentBranchId.HasValue)
                {
                    if (att.BranchId != CurrentBranchId.Value)
                        return ErrorResult("Permission denied.");
                }

                _attendanceService.DeleteAttendance(id);
                return SuccessResult("Attendance deleted successfully.");
            }
            catch (Exception ex)
            {
                return ErrorResult("Error: " + ex.Message);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) _context.Dispose();
            base.Dispose(disposing);
        }
    }
}