using System;
using System.Collections.Generic;
using System.Linq;
using TEXCEL_ERP.Data.Repository;
using TEXCEL_ERP.Models.Entities;
using TEXCEL_ERP.Models.Enums;
using TEXCEL_ERP.Models.ViewModels;

namespace TEXCEL_ERP.Services
{
    /// <summary>
    /// Service implementation for Attendance Reports.
    /// </summary>
    public class AttendanceReportService : IAttendanceReportService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AttendanceReportService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // =============================================
        // 1. DAILY REPORT
        // =============================================
        public DailyReportViewModel GetDailyReport(DateTime date, int? branchId, int? departmentId)
        {
            var model = new DailyReportViewModel
            {
                ReportDate = date.Date,
                BranchId = branchId,
                DepartmentId = departmentId
            };

            // Branch name
            if (branchId.HasValue)
            {
                var branch = _unitOfWork.Branches.GetById(branchId.Value);
                model.BranchName = branch != null ? branch.BranchName : "All Branches";
            }
            else
            {
                model.BranchName = "All Branches";
            }

            // Department name
            if (departmentId.HasValue)
            {
                var dept = _unitOfWork.Departments.GetById(departmentId.Value);
                model.DepartmentName = dept != null ? dept.DepartmentName : "All Departments";
            }
            else
            {
                model.DepartmentName = "All Departments";
            }

            // Employees Query
            var employeesQuery = _unitOfWork.Employees.Find(e => e.IsActive);

            if (branchId.HasValue)
            {
                int bid = branchId.Value;
                employeesQuery = employeesQuery.Where(e => e.BranchId == bid);
            }

            if (departmentId.HasValue)
            {
                int did = departmentId.Value;
                employeesQuery = employeesQuery.Where(e => e.DepartmentId == did);
            }

            var employees = employeesQuery.ToList();
            var employeeIds = employees.Select(e => e.EmployeeId).ToList();

            model.TotalEmployees = employees.Count;

            // Attendance Query
            var attendanceList = _unitOfWork.Attendances
                .Find(a => a.AttendanceDate == date.Date && employeeIds.Contains(a.EmployeeId))
                .ToList();

            // KPI Calculations
            model.PresentCount = attendanceList.Count(a => a.Status == AttendanceStatus.Present);
            model.AbsentCount = attendanceList.Count(a => a.Status == AttendanceStatus.Absent);
            model.LeaveCount = attendanceList.Count(a => a.Status == AttendanceStatus.Leave);
            model.HalfDayCount = attendanceList.Count(a => a.Status == AttendanceStatus.HalfDay);
            model.HolidayCount = attendanceList.Count(a => a.Status == AttendanceStatus.Holiday);
            model.LateCount = attendanceList.Count(a => a.LateMinutes > 0);
            model.TotalOvertimeHours = attendanceList.Sum(a => a.OvertimeHours);
            model.TotalLessHours = attendanceList.Sum(a => a.LessTimeHours);

            if (model.TotalEmployees > 0)
            {
                model.AttendancePercentage = Math.Round(
                    (decimal)model.PresentCount / model.TotalEmployees * 100, 1);
            }

            // Chart Data — Status Distribution
            model.StatusDistribution = new List<ReportChartData>
            {
                new ReportChartData { Label = "Present",  Value = model.PresentCount,  Color = "#10b981" },
                new ReportChartData { Label = "Absent",   Value = model.AbsentCount,   Color = "#ef4444" },
                new ReportChartData { Label = "Leave",    Value = model.LeaveCount,    Color = "#3b82f6" },
                new ReportChartData { Label = "Half Day", Value = model.HalfDayCount,  Color = "#f59e0b" },
                new ReportChartData { Label = "Holiday",  Value = model.HolidayCount,  Color = "#8b5cf6" }
            };

            // Department-wise Breakdown
            var deptGroups = employees
                .GroupBy(e => new { e.DepartmentId, DeptName = e.Department != null ? e.Department.DepartmentName : "Unknown" })
                .Select(g => new ReportChartData
                {
                    Label = g.Key.DeptName,
                    Value = attendanceList.Count(a => g.Select(e => e.EmployeeId).Contains(a.EmployeeId)
                        && a.Status == AttendanceStatus.Present),
                    Color = "#8b5cf6"
                })
                .ToList();

            model.DepartmentWiseBreakdown = deptGroups;

            // Detail Rows
            var detailRows = new List<DailyAttendanceRow>();

            foreach (var emp in employees)
            {
                var att = attendanceList.FirstOrDefault(a => a.EmployeeId == emp.EmployeeId);

                detailRows.Add(new DailyAttendanceRow
                {
                    EmployeeId = emp.EmployeeId,
                    EmployeeCode = emp.EmployeeCode,
                    EmployeeName = emp.FullName,
                    DepartmentName = emp.Department != null ? emp.Department.DepartmentName : "-",
                    DesignationName = emp.Designation != null ? emp.Designation.DesignationName : "-",
                    ShiftName = emp.Shift != null ? emp.Shift.ShiftName : "-",
                    CheckIn = att != null ? att.CheckIn : null,
                    CheckOut = att != null ? att.CheckOut : null,
                    WorkedHours = att != null ? att.WorkedHours : 0,
                    OvertimeHours = att != null ? att.OvertimeHours : 0,
                    LessTimeHours = att != null ? att.LessTimeHours : 0,
                    LateMinutes = att != null ? att.LateMinutes : 0,
                    EarlyGoingMinutes = att != null ? att.EarlyGoingMinutes : 0,
                    Status = att != null ? att.Status : AttendanceStatus.Absent,
                    StatusText = att != null ? GetStatusText(att.Status) : "Absent"
                });
            }

            model.AttendanceDetails = detailRows.OrderBy(r => r.EmployeeCode).ToList();

            // Filter Dropdowns
            model.Branches = _unitOfWork.Branches.Find(b => b.IsActive).ToList();

            if (branchId.HasValue)
            {
                int bid = branchId.Value;
                model.Departments = _unitOfWork.Departments.Find(d => d.IsActive && d.BranchId == bid).ToList();
            }
            else
            {
                model.Departments = _unitOfWork.Departments.Find(d => d.IsActive).ToList();
            }

            return model;
        }

        // =============================================
        // 2. MONTHLY SUMMARY REPORT
        // =============================================
        public MonthlySummaryReportViewModel GetMonthlySummaryReport(int month, int year, int? branchId, int? departmentId)
        {
            var model = new MonthlySummaryReportViewModel
            {
                Month = month,
                Year = year,
                MonthName = new DateTime(year, month, 1).ToString("MMMM yyyy"),
                BranchId = branchId,
                DepartmentId = departmentId
            };

            // Branch name
            if (branchId.HasValue)
            {
                var branch = _unitOfWork.Branches.GetById(branchId.Value);
                model.BranchName = branch != null ? branch.BranchName : "All Branches";
            }
            else
            {
                model.BranchName = "All Branches";
            }

            // Department name
            if (departmentId.HasValue)
            {
                var dept = _unitOfWork.Departments.GetById(departmentId.Value);
                model.DepartmentName = dept != null ? dept.DepartmentName : "All Departments";
            }
            else
            {
                model.DepartmentName = "All Departments";
            }

            // Employees Query
            var employeesQuery = _unitOfWork.Employees.Find(e => e.IsActive);

            if (branchId.HasValue)
            {
                int bid = branchId.Value;
                employeesQuery = employeesQuery.Where(e => e.BranchId == bid);
            }

            if (departmentId.HasValue)
            {
                int did = departmentId.Value;
                employeesQuery = employeesQuery.Where(e => e.DepartmentId == did);
            }

            var employees = employeesQuery.ToList();
            var employeeIds = employees.Select(e => e.EmployeeId).ToList();

            model.TotalEmployees = employees.Count;

            // Attendance Query — whole month
            var attendanceList = _unitOfWork.Attendances
                .Find(a => a.AttendanceDate.Month == month
                    && a.AttendanceDate.Year == year
                    && employeeIds.Contains(a.EmployeeId))
                .ToList();

            // Totals
            model.TotalPresentDays = attendanceList.Count(a => a.Status == AttendanceStatus.Present);
            model.TotalAbsentDays = attendanceList.Count(a => a.Status == AttendanceStatus.Absent);
            model.TotalLeaveDays = attendanceList.Count(a => a.Status == AttendanceStatus.Leave);
            model.TotalOvertimeHours = attendanceList.Sum(a => a.OvertimeHours);
            model.TotalLessHours = attendanceList.Sum(a => a.LessTimeHours);
            model.TotalLateMinutes = attendanceList.Sum(a => a.LateMinutes);

            // Employee-wise summary
            var summaries = new List<MonthlySummaryRow>();

            foreach (var emp in employees)
            {
                var empAtt = attendanceList.Where(a => a.EmployeeId == emp.EmployeeId).ToList();

                var presentDays = empAtt.Count(a => a.Status == AttendanceStatus.Present);
                var absentDays = empAtt.Count(a => a.Status == AttendanceStatus.Absent);
                var leaveDays = empAtt.Count(a => a.Status == AttendanceStatus.Leave);
                var halfDays = empAtt.Count(a => a.Status == AttendanceStatus.HalfDay);
                var holidays = empAtt.Count(a => a.Status == AttendanceStatus.Holiday);
                var lateCount = empAtt.Count(a => a.LateMinutes > 0);

                var totalWorkingDays = presentDays + absentDays + leaveDays + halfDays;
                var attendancePct = totalWorkingDays > 0
                    ? Math.Round((decimal)(presentDays + (halfDays * 0.5m)) / totalWorkingDays * 100, 1)
                    : 0;

                summaries.Add(new MonthlySummaryRow
                {
                    EmployeeId = emp.EmployeeId,
                    EmployeeCode = emp.EmployeeCode,
                    EmployeeName = emp.FullName,
                    DepartmentName = emp.Department != null ? emp.Department.DepartmentName : "-",
                    DesignationName = emp.Designation != null ? emp.Designation.DesignationName : "-",
                    ShiftName = emp.Shift != null ? emp.Shift.ShiftName : "-",
                    PresentDays = presentDays,
                    AbsentDays = absentDays,
                    LeaveDays = leaveDays,
                    HalfDays = halfDays,
                    Holidays = holidays,
                    LateCount = lateCount,
                    TotalWorkedHours = empAtt.Sum(a => a.WorkedHours),
                    TotalOvertimeHours = empAtt.Sum(a => a.OvertimeHours),
                    TotalLessHours = empAtt.Sum(a => a.LessTimeHours),
                    TotalLateMinutes = empAtt.Sum(a => a.LateMinutes),
                    TotalEarlyGoingMinutes = empAtt.Sum(a => a.EarlyGoingMinutes),
                    AttendancePercentage = attendancePct
                });
            }

            model.EmployeeSummaries = summaries.OrderBy(s => s.EmployeeCode).ToList();

            // Average attendance
            if (summaries.Any())
            {
                model.AverageAttendancePercentage = Math.Round(summaries.Average(s => s.AttendancePercentage), 1);
            }

            // Top 5 Present Employees
            model.TopPresentEmployees = summaries
                .OrderByDescending(s => s.PresentDays)
                .Take(5)
                .Select(s => new ReportChartData
                {
                    Label = s.EmployeeName,
                    Value = s.PresentDays,
                    Color = "#10b981"
                })
                .ToList();

            // Top 5 OT Employees
            model.TopOvertimeEmployees = summaries
                .Where(s => s.TotalOvertimeHours > 0)
                .OrderByDescending(s => s.TotalOvertimeHours)
                .Take(5)
                .Select(s => new ReportChartData
                {
                    Label = s.EmployeeName,
                    Value = s.TotalOvertimeHours,
                    Color = "#f59e0b"
                })
                .ToList();

            // Filter Dropdowns
            model.Branches = _unitOfWork.Branches.Find(b => b.IsActive).ToList();

            if (branchId.HasValue)
            {
                int bid = branchId.Value;
                model.Departments = _unitOfWork.Departments.Find(d => d.IsActive && d.BranchId == bid).ToList();
            }
            else
            {
                model.Departments = _unitOfWork.Departments.Find(d => d.IsActive).ToList();
            }

            return model;
        }

        // =============================================
        // 3. LATE & OT REPORT
        // =============================================
        public LateOTReportViewModel GetLateOTReport(DateTime fromDate, DateTime toDate, int? branchId, int? departmentId)
        {
            var model = new LateOTReportViewModel
            {
                FromDate = fromDate.Date,
                ToDate = toDate.Date,
                BranchId = branchId,
                DepartmentId = departmentId
            };

            // Branch name
            if (branchId.HasValue)
            {
                var branch = _unitOfWork.Branches.GetById(branchId.Value);
                model.BranchName = branch != null ? branch.BranchName : "All Branches";
            }
            else
            {
                model.BranchName = "All Branches";
            }

            // Department name
            if (departmentId.HasValue)
            {
                var dept = _unitOfWork.Departments.GetById(departmentId.Value);
                model.DepartmentName = dept != null ? dept.DepartmentName : "All Departments";
            }
            else
            {
                model.DepartmentName = "All Departments";
            }

            // Employees
            var employeesQuery = _unitOfWork.Employees.Find(e => e.IsActive);

            if (branchId.HasValue)
            {
                int bid = branchId.Value;
                employeesQuery = employeesQuery.Where(e => e.BranchId == bid);
            }

            if (departmentId.HasValue)
            {
                int did = departmentId.Value;
                employeesQuery = employeesQuery.Where(e => e.DepartmentId == did);
            }

            var employees = employeesQuery.ToList();
            var employeeIds = employees.Select(e => e.EmployeeId).ToList();

            // Attendance Query
            var attendanceList = _unitOfWork.Attendances
                .Find(a => a.AttendanceDate >= fromDate.Date
                    && a.AttendanceDate <= toDate.Date
                    && employeeIds.Contains(a.EmployeeId))
                .OrderBy(a => a.AttendanceDate)
                .ToList();

            // KPI
            var lateRecords = attendanceList.Where(a => a.LateMinutes > 0).ToList();
            var otRecords = attendanceList.Where(a => a.OvertimeHours > 0).ToList();

            model.TotalLateInstances = lateRecords.Count;
            model.TotalLateMinutes = lateRecords.Sum(a => a.LateMinutes);
            model.TotalOvertimeHours = otRecords.Sum(a => a.OvertimeHours);
            model.EmployeesWithLate = lateRecords.Select(a => a.EmployeeId).Distinct().Count();
            model.EmployeesWithOT = otRecords.Select(a => a.EmployeeId).Distinct().Count();

            if (model.EmployeesWithOT > 0)
            {
                model.AverageOTPerEmployee = Math.Round(model.TotalOvertimeHours / model.EmployeesWithOT, 2);
            }

            // Late Records
            model.LateRecords = lateRecords.Select(a =>
            {
                var emp = employees.FirstOrDefault(e => e.EmployeeId == a.EmployeeId);
                var shift = emp != null && emp.ShiftId.HasValue ? _unitOfWork.Shifts.GetById(emp.ShiftId.Value) : null;

                return new LateReportRow
                {
                    AttendanceId = a.AttendanceId,
                    EmployeeCode = emp != null ? emp.EmployeeCode : "-",
                    EmployeeName = emp != null ? emp.FullName : "-",
                    DepartmentName = emp != null && emp.Department != null ? emp.Department.DepartmentName : "-",
                    AttendanceDate = a.AttendanceDate,
                    CheckIn = a.CheckIn,
                    ShiftStart = shift != null ? shift.StartTime : TimeSpan.Zero,
                    LateMinutes = a.LateMinutes
                };
            })
            .OrderByDescending(r => r.LateMinutes)
            .ToList();

            // OT Records
            model.OTRecords = otRecords.Select(a =>
            {
                var emp = employees.FirstOrDefault(e => e.EmployeeId == a.EmployeeId);
                var shift = emp != null && emp.ShiftId.HasValue ? _unitOfWork.Shifts.GetById(emp.ShiftId.Value) : null;

                // OT Amount = (BasicSalary / (30 * ShiftHours)) * OT * 1.5
                decimal otAmount = 0;
                if (emp != null && shift != null)
                {
                    var shiftHours = (shift.EndTime - shift.StartTime).TotalHours - (shift.BreakDuration / 60.0);
                    if (shiftHours > 0 && emp.BasicSalary > 0)
                    {
                        var perHourRate = emp.BasicSalary / (30 * (decimal)shiftHours);
                        otAmount = Math.Round(perHourRate * a.OvertimeHours * shift.OvertimeRate, 2);
                    }
                }

                return new OTReportRow
                {
                    AttendanceId = a.AttendanceId,
                    EmployeeCode = emp != null ? emp.EmployeeCode : "-",
                    EmployeeName = emp != null ? emp.FullName : "-",
                    DepartmentName = emp != null && emp.Department != null ? emp.Department.DepartmentName : "-",
                    AttendanceDate = a.AttendanceDate,
                    CheckOut = a.CheckOut,
                    ShiftEnd = shift != null ? shift.EndTime : TimeSpan.Zero,
                    OvertimeHours = a.OvertimeHours,
                    OvertimeAmount = otAmount
                };
            })
            .OrderByDescending(r => r.OvertimeHours)
            .ToList();

            // Charts — Daily Trend
            var dateRange = new List<DateTime>();
            for (var d = fromDate.Date; d <= toDate.Date; d = d.AddDays(1))
            {
                dateRange.Add(d);
            }

            model.LateTrendChart = dateRange.Select(d => new ReportChartData
            {
                Label = d.ToString("dd MMM"),
                Value = lateRecords.Where(a => a.AttendanceDate == d).Sum(a => a.LateMinutes),
                Color = "#ef4444"
            }).ToList();

            model.OTTrendChart = dateRange.Select(d => new ReportChartData
            {
                Label = d.ToString("dd MMM"),
                Value = otRecords.Where(a => a.AttendanceDate == d).Sum(a => a.OvertimeHours),
                Color = "#10b981"
            }).ToList();

            // Filter Dropdowns
            model.Branches = _unitOfWork.Branches.Find(b => b.IsActive).ToList();

            if (branchId.HasValue)
            {
                int bid = branchId.Value;
                model.Departments = _unitOfWork.Departments.Find(d => d.IsActive && d.BranchId == bid).ToList();
            }
            else
            {
                model.Departments = _unitOfWork.Departments.Find(d => d.IsActive).ToList();
            }

            return model;
        }

        // =============================================
        // 4. DEPARTMENT-WISE REPORT
        // =============================================
        public DepartmentReportViewModel GetDepartmentReport(DateTime fromDate, DateTime toDate, int? branchId)
        {
            var model = new DepartmentReportViewModel
            {
                FromDate = fromDate.Date,
                ToDate = toDate.Date,
                BranchId = branchId
            };

            // Branch name
            if (branchId.HasValue)
            {
                var branch = _unitOfWork.Branches.GetById(branchId.Value);
                model.BranchName = branch != null ? branch.BranchName : "All Branches";
            }
            else
            {
                model.BranchName = "All Branches";
            }

            // Departments
            var deptQuery = _unitOfWork.Departments.Find(d => d.IsActive);

            if (branchId.HasValue)
            {
                int bid = branchId.Value;
                deptQuery = deptQuery.Where(d => d.BranchId == bid);
            }

            var departments = deptQuery.ToList();
            model.TotalDepartments = departments.Count;

            // Build rows
            var rows = new List<DepartmentReportRow>();

            foreach (var dept in departments)
            {
                var emps = _unitOfWork.Employees
                    .Find(e => e.IsActive && e.DepartmentId == dept.DepartmentId)
                    .ToList();

                var empIds = emps.Select(e => e.EmployeeId).ToList();

                var atts = _unitOfWork.Attendances
                    .Find(a => a.AttendanceDate >= fromDate.Date
                        && a.AttendanceDate <= toDate.Date
                        && empIds.Contains(a.EmployeeId))
                    .ToList();

                var present = atts.Count(a => a.Status == AttendanceStatus.Present);
                var absent = atts.Count(a => a.Status == AttendanceStatus.Absent);
                var leave = atts.Count(a => a.Status == AttendanceStatus.Leave);

                var totalDays = present + absent + leave;
                var pct = totalDays > 0 ? Math.Round((decimal)present / totalDays * 100, 1) : 0;

                rows.Add(new DepartmentReportRow
                {
                    DepartmentId = dept.DepartmentId,
                    DepartmentName = dept.DepartmentName,
                    DepartmentCode = dept.DepartmentCode,
                    TotalEmployees = emps.Count,
                    TotalPresentDays = present,
                    TotalAbsentDays = absent,
                    TotalLeaveDays = leave,
                    TotalOvertimeHours = atts.Sum(a => a.OvertimeHours),
                    TotalLessHours = atts.Sum(a => a.LessTimeHours),
                    TotalLateMinutes = atts.Sum(a => a.LateMinutes),
                    AttendancePercentage = pct
                });
            }

            model.Departments = rows.OrderByDescending(r => r.AttendancePercentage).ToList();

            // Overall
            if (rows.Any())
            {
                model.OverallAttendancePercentage = Math.Round(rows.Average(r => r.AttendancePercentage), 1);
                model.BestDepartment = rows.OrderByDescending(r => r.AttendancePercentage).First().DepartmentName;
                model.WorstDepartment = rows.OrderBy(r => r.AttendancePercentage).First().DepartmentName;
            }

            // Charts
            model.DepartmentAttendanceChart = rows.Select(r => new ReportChartData
            {
                Label = r.DepartmentName,
                Value = r.AttendancePercentage,
                Color = "#10b981"
            }).ToList();

            model.DepartmentOvertimeChart = rows.Select(r => new ReportChartData
            {
                Label = r.DepartmentName,
                Value = r.TotalOvertimeHours,
                Color = "#f59e0b"
            }).ToList();

            // Filter Dropdowns
            model.Branches = _unitOfWork.Branches.Find(b => b.IsActive).ToList();

            return model;
        }

        // =============================================
        // 5. GRAPH ANALYTICS
        // =============================================
        public GraphAnalyticsViewModel GetGraphAnalytics(int month, int year, int? branchId)
        {
            var model = new GraphAnalyticsViewModel
            {
                Month = month,
                Year = year,
                MonthName = new DateTime(year, month, 1).ToString("MMMM yyyy"),
                BranchId = branchId
            };

            if (branchId.HasValue)
            {
                var branch = _unitOfWork.Branches.GetById(branchId.Value);
                model.BranchName = branch != null ? branch.BranchName : "All Branches";
            }
            else
            {
                model.BranchName = "All Branches";
            }

            // Employees
            var empQuery = _unitOfWork.Employees.Find(e => e.IsActive);

            if (branchId.HasValue)
            {
                int bid = branchId.Value;
                empQuery = empQuery.Where(e => e.BranchId == bid);
            }

            var employees = empQuery.ToList();
            var empIds = employees.Select(e => e.EmployeeId).ToList();

            // Month attendance
            var monthAtt = _unitOfWork.Attendances
                .Find(a => a.AttendanceDate.Month == month
                    && a.AttendanceDate.Year == year
                    && empIds.Contains(a.EmployeeId))
                .ToList();

            // Chart 1: Employee-wise Present (Bar)
            model.EmployeePresentChart = employees
                .Select(e => new ReportChartData
                {
                    Label = e.FirstName + " " + (e.LastName ?? ""),
                    Value = monthAtt.Count(a => a.EmployeeId == e.EmployeeId && a.Status == AttendanceStatus.Present),
                    Color = "#8b5cf6"
                })
                .OrderByDescending(x => x.Value)
                .Take(10)
                .ToList();

            // Chart 2: Monthly Trend — full year
            var monthlyTrend = new List<ReportChartData>();
            for (int m = 1; m <= 12; m++)
            {
                var total = _unitOfWork.Attendances
                    .Find(a => a.AttendanceDate.Month == m
                        && a.AttendanceDate.Year == year
                        && empIds.Contains(a.EmployeeId)
                        && a.Status == AttendanceStatus.Present)
                    .Count();

                monthlyTrend.Add(new ReportChartData
                {
                    Label = new DateTime(year, m, 1).ToString("MMM"),
                    Value = total,
                    Color = "#3b82f6"
                });
            }
            model.MonthlyTrendChart = monthlyTrend;

            // Chart 3: Status Distribution (Pie)
            model.StatusDistributionChart = new List<ReportChartData>
            {
                new ReportChartData { Label = "Present", Value = monthAtt.Count(a => a.Status == AttendanceStatus.Present), Color = "#10b981" },
                new ReportChartData { Label = "Absent", Value = monthAtt.Count(a => a.Status == AttendanceStatus.Absent), Color = "#ef4444" },
                new ReportChartData { Label = "Leave", Value = monthAtt.Count(a => a.Status == AttendanceStatus.Leave), Color = "#3b82f6" },
                new ReportChartData { Label = "Half Day", Value = monthAtt.Count(a => a.Status == AttendanceStatus.HalfDay), Color = "#f59e0b" }
            };

            // Chart 4: Department Comparison (Stacked Bar)
            var depts = _unitOfWork.Departments.Find(d => d.IsActive).ToList();

            if (branchId.HasValue)
            {
                int bid = branchId.Value;
                depts = depts.Where(d => d.BranchId == bid).ToList();
            }

            var deptComparison = new List<ReportMultiSeriesChartData>();
            var deptLabels = new List<string>();

            foreach (var dept in depts.Take(6))
            {
                var deptEmpIds = employees
                    .Where(e => e.DepartmentId == dept.DepartmentId)
                    .Select(e => e.EmployeeId)
                    .ToList();

                var deptAtt = monthAtt.Where(a => deptEmpIds.Contains(a.EmployeeId)).ToList();

                deptComparison.Add(new ReportMultiSeriesChartData
                {
                    Label = dept.DepartmentName,
                    Values = new List<decimal>
                    {
                        deptAtt.Count(a => a.Status == AttendanceStatus.Present),
                        deptAtt.Count(a => a.Status == AttendanceStatus.Absent),
                        deptAtt.Count(a => a.Status == AttendanceStatus.Leave)
                    }
                });

                deptLabels.Add(dept.DepartmentName);
            }

            model.DepartmentComparisonChart = deptComparison;
            model.DepartmentComparisonLabels = deptLabels;
            model.DepartmentComparisonSeries = new List<string> { "Present", "Absent", "Leave" };

            // Filter Dropdowns
            model.Branches = _unitOfWork.Branches.Find(b => b.IsActive).ToList();

            return model;
        }

        // =============================================
        // 6. FULL ATTENDANCE REPORT
        // =============================================
        public FullAttendanceReportViewModel GetFullAttendanceReport(
            DateTime fromDate, DateTime toDate,
            int? branchId, int? departmentId, int? employeeId, AttendanceStatus? status)
        {
            var model = new FullAttendanceReportViewModel
            {
                FromDate = fromDate.Date,
                ToDate = toDate.Date,
                BranchId = branchId,
                DepartmentId = departmentId,
                EmployeeId = employeeId,
                Status = status
            };

            // Employees
            var empQuery = _unitOfWork.Employees.Find(e => e.IsActive);

            if (branchId.HasValue)
            {
                int bid = branchId.Value;
                empQuery = empQuery.Where(e => e.BranchId == bid);
            }

            if (departmentId.HasValue)
            {
                int did = departmentId.Value;
                empQuery = empQuery.Where(e => e.DepartmentId == did);
            }

            if (employeeId.HasValue)
            {
                int eid = employeeId.Value;
                empQuery = empQuery.Where(e => e.EmployeeId == eid);
            }

            var employees = empQuery.ToList();
            var empIds = employees.Select(e => e.EmployeeId).ToList();

            // Attendance
            var attQuery = _unitOfWork.Attendances
                .Find(a => a.AttendanceDate >= fromDate.Date
                    && a.AttendanceDate <= toDate.Date
                    && empIds.Contains(a.EmployeeId));

            if (status.HasValue)
            {
                attQuery = attQuery.Where(a => a.Status == status.Value);
            }

            var attendanceList = attQuery.OrderByDescending(a => a.AttendanceDate).ToList();

            // KPI
            model.TotalRecords = attendanceList.Count;
            model.TotalWorkedHours = attendanceList.Sum(a => a.WorkedHours);
            model.TotalOvertimeHours = attendanceList.Sum(a => a.OvertimeHours);
            model.TotalLessHours = attendanceList.Sum(a => a.LessTimeHours);

            // Rows
            model.Records = attendanceList.Select(a =>
            {
                var emp = employees.FirstOrDefault(e => e.EmployeeId == a.EmployeeId);
                var shift = emp != null && emp.ShiftId.HasValue ? _unitOfWork.Shifts.GetById(emp.ShiftId.Value) : null;

                return new FullAttendanceRow
                {
                    AttendanceId = a.AttendanceId,
                    EmployeeCode = emp != null ? emp.EmployeeCode : "-",
                    EmployeeName = emp != null ? emp.FullName : "-",
                    BranchName = emp != null && emp.Branch != null ? emp.Branch.BranchName : "-",
                    DepartmentName = emp != null && emp.Department != null ? emp.Department.DepartmentName : "-",
                    DesignationName = emp != null && emp.Designation != null ? emp.Designation.DesignationName : "-",
                    ShiftName = shift != null ? shift.ShiftName : "-",
                    AttendanceDate = a.AttendanceDate,
                    CheckIn = a.CheckIn,
                    CheckOut = a.CheckOut,
                    WorkedHours = a.WorkedHours,
                    OvertimeHours = a.OvertimeHours,
                    LessTimeHours = a.LessTimeHours,
                    LateMinutes = a.LateMinutes,
                    EarlyGoingMinutes = a.EarlyGoingMinutes,
                    Status = a.Status,
                    StatusText = GetStatusText(a.Status),
                    Remarks = a.Remarks
                };
            }).ToList();

            // Filter Dropdowns
            model.Branches = _unitOfWork.Branches.Find(b => b.IsActive).ToList();

            if (branchId.HasValue)
            {
                int bid = branchId.Value;
                model.Departments = _unitOfWork.Departments.Find(d => d.IsActive && d.BranchId == bid).ToList();
                model.Employees = _unitOfWork.Employees.Find(e => e.IsActive && e.BranchId == bid).ToList();
            }
            else
            {
                model.Departments = _unitOfWork.Departments.Find(d => d.IsActive).ToList();
                model.Employees = _unitOfWork.Employees.Find(e => e.IsActive).ToList();
            }

            return model;
        }

        // =============================================
        // HELPERS
        // =============================================
        private string GetStatusText(AttendanceStatus status)
        {
            switch (status)
            {
                case AttendanceStatus.Present: return "Present";
                case AttendanceStatus.Absent: return "Absent";
                case AttendanceStatus.Leave: return "Leave";
                case AttendanceStatus.HalfDay: return "Half Day";
                case AttendanceStatus.Holiday: return "Holiday";
                case AttendanceStatus.Weekend: return "Weekend";
                default: return "Unknown";
            }
        }
    }
}