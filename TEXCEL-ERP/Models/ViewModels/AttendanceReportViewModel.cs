using System;
using System.Collections.Generic;
using TEXCEL_ERP.Models.Entities;
using TEXCEL_ERP.Models.Enums;

namespace TEXCEL_ERP.Models.ViewModels
{
    // =============================================
    // COMMON CLASSES
    // =============================================

    /// <summary>
    /// Common KPI Card for reports.
    /// </summary>
    public class ReportKPI
    {
        public string Label { get; set; }
        public string Value { get; set; }
        public string Icon { get; set; }
        public string Color { get; set; }
        public string SubText { get; set; }
    }

    /// <summary>
    /// Chart data item (label + value).
    /// </summary>
    public class ReportChartData
    {
        public string Label { get; set; }
        public decimal Value { get; set; }
        public string Color { get; set; }
    }

    /// <summary>
    /// Multi-series chart data (for stacked bars, etc.)
    /// </summary>
    public class ReportMultiSeriesChartData
    {
        public string Label { get; set; }
        public List<decimal> Values { get; set; }
    }

    // =============================================
    // 1. DAILY REPORT
    // =============================================

    public class DailyReportViewModel
    {
        public DateTime ReportDate { get; set; }
        public int? BranchId { get; set; }
        public string BranchName { get; set; }
        public int? DepartmentId { get; set; }
        public string DepartmentName { get; set; }

        // KPI Cards
        public int TotalEmployees { get; set; }
        public int PresentCount { get; set; }
        public int AbsentCount { get; set; }
        public int LeaveCount { get; set; }
        public int HalfDayCount { get; set; }
        public int HolidayCount { get; set; }
        public int LateCount { get; set; }
        public decimal TotalOvertimeHours { get; set; }
        public decimal TotalLessHours { get; set; }
        public decimal AttendancePercentage { get; set; }

        // Chart Data
        public List<ReportChartData> StatusDistribution { get; set; }
        public List<ReportChartData> DepartmentWiseBreakdown { get; set; }

        // Detail Data
        public List<DailyAttendanceRow> AttendanceDetails { get; set; }

        // Filter Dropdowns
        public List<Branch> Branches { get; set; }
        public List<Department> Departments { get; set; }

        public DailyReportViewModel()
        {
            StatusDistribution = new List<ReportChartData>();
            DepartmentWiseBreakdown = new List<ReportChartData>();
            AttendanceDetails = new List<DailyAttendanceRow>();
            Branches = new List<Branch>();
            Departments = new List<Department>();
        }
    }

    public class DailyAttendanceRow
    {
        public int EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public string ShiftName { get; set; }
        public TimeSpan? CheckIn { get; set; }
        public TimeSpan? CheckOut { get; set; }
        public decimal WorkedHours { get; set; }
        public decimal OvertimeHours { get; set; }
        public decimal LessTimeHours { get; set; }
        public int LateMinutes { get; set; }
        public int EarlyGoingMinutes { get; set; }
        public AttendanceStatus Status { get; set; }
        public string StatusText { get; set; }
    }

    // =============================================
    // 2. MONTHLY SUMMARY REPORT
    // =============================================

    public class MonthlySummaryReportViewModel
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public string MonthName { get; set; }
        public int? BranchId { get; set; }
        public string BranchName { get; set; }
        public int? DepartmentId { get; set; }
        public string DepartmentName { get; set; }

        // KPI Cards
        public int TotalEmployees { get; set; }
        public decimal TotalPresentDays { get; set; }
        public decimal TotalAbsentDays { get; set; }
        public decimal TotalLeaveDays { get; set; }
        public decimal TotalOvertimeHours { get; set; }
        public decimal TotalLessHours { get; set; }
        public int TotalLateMinutes { get; set; }
        public decimal AverageAttendancePercentage { get; set; }

        // Detail Rows
        public List<MonthlySummaryRow> EmployeeSummaries { get; set; }

        // Chart Data
        public List<ReportChartData> TopPresentEmployees { get; set; }
        public List<ReportChartData> TopOvertimeEmployees { get; set; }

        // Filter Dropdowns
        public List<Branch> Branches { get; set; }
        public List<Department> Departments { get; set; }

        public MonthlySummaryReportViewModel()
        {
            EmployeeSummaries = new List<MonthlySummaryRow>();
            TopPresentEmployees = new List<ReportChartData>();
            TopOvertimeEmployees = new List<ReportChartData>();
            Branches = new List<Branch>();
            Departments = new List<Department>();
        }
    }

    public class MonthlySummaryRow
    {
        public int EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public string ShiftName { get; set; }

        public int PresentDays { get; set; }
        public int AbsentDays { get; set; }
        public int LeaveDays { get; set; }
        public int HalfDays { get; set; }
        public int Holidays { get; set; }
        public int LateCount { get; set; }

        public decimal TotalWorkedHours { get; set; }
        public decimal TotalOvertimeHours { get; set; }
        public decimal TotalLessHours { get; set; }
        public int TotalLateMinutes { get; set; }
        public int TotalEarlyGoingMinutes { get; set; }

        public decimal AttendancePercentage { get; set; }
    }

    // =============================================
    // 3. LATE & OT REPORT
    // =============================================

    public class LateOTReportViewModel
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int? BranchId { get; set; }
        public string BranchName { get; set; }
        public int? DepartmentId { get; set; }
        public string DepartmentName { get; set; }

        // KPI Cards
        public int TotalLateInstances { get; set; }
        public int TotalLateMinutes { get; set; }
        public decimal TotalOvertimeHours { get; set; }
        public int EmployeesWithOT { get; set; }
        public int EmployeesWithLate { get; set; }
        public decimal AverageOTPerEmployee { get; set; }

        // Late Rows
        public List<LateReportRow> LateRecords { get; set; }

        // OT Rows
        public List<OTReportRow> OTRecords { get; set; }

        // Chart Data
        public List<ReportChartData> LateTrendChart { get; set; }
        public List<ReportChartData> OTTrendChart { get; set; }

        // Filter Dropdowns
        public List<Branch> Branches { get; set; }
        public List<Department> Departments { get; set; }

        public LateOTReportViewModel()
        {
            LateRecords = new List<LateReportRow>();
            OTRecords = new List<OTReportRow>();
            LateTrendChart = new List<ReportChartData>();
            OTTrendChart = new List<ReportChartData>();
            Branches = new List<Branch>();
            Departments = new List<Department>();
        }
    }

    public class LateReportRow
    {
        public int AttendanceId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string DepartmentName { get; set; }
        public DateTime AttendanceDate { get; set; }
        public TimeSpan? CheckIn { get; set; }
        public TimeSpan ShiftStart { get; set; }
        public int LateMinutes { get; set; }
    }

    public class OTReportRow
    {
        public int AttendanceId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string DepartmentName { get; set; }
        public DateTime AttendanceDate { get; set; }
        public TimeSpan? CheckOut { get; set; }
        public TimeSpan ShiftEnd { get; set; }
        public decimal OvertimeHours { get; set; }
        public decimal OvertimeAmount { get; set; }
    }

    // =============================================
    // 4. DEPARTMENT-WISE REPORT
    // =============================================

    public class DepartmentReportViewModel
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int? BranchId { get; set; }
        public string BranchName { get; set; }

        // KPI Cards
        public int TotalDepartments { get; set; }
        public decimal OverallAttendancePercentage { get; set; }
        public string BestDepartment { get; set; }
        public string WorstDepartment { get; set; }

        // Department Rows
        public List<DepartmentReportRow> Departments { get; set; }

        // Chart Data
        public List<ReportChartData> DepartmentAttendanceChart { get; set; }
        public List<ReportChartData> DepartmentOvertimeChart { get; set; }

        // Filter Dropdowns
        public List<Branch> Branches { get; set; }

        public DepartmentReportViewModel()
        {
            Departments = new List<DepartmentReportRow>();
            DepartmentAttendanceChart = new List<ReportChartData>();
            DepartmentOvertimeChart = new List<ReportChartData>();
            Branches = new List<Branch>();
        }
    }

    public class DepartmentReportRow
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentCode { get; set; }
        public int TotalEmployees { get; set; }
        public int TotalPresentDays { get; set; }
        public int TotalAbsentDays { get; set; }
        public int TotalLeaveDays { get; set; }
        public decimal TotalOvertimeHours { get; set; }
        public decimal TotalLessHours { get; set; }
        public int TotalLateMinutes { get; set; }
        public decimal AttendancePercentage { get; set; }
    }

    // =============================================
    // 5. GRAPH ANALYTICS
    // =============================================

    public class GraphAnalyticsViewModel
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public string MonthName { get; set; }
        public int? BranchId { get; set; }
        public string BranchName { get; set; }

        // Chart Data — multiple charts
        public List<ReportChartData> EmployeePresentChart { get; set; }       // Bar
        public List<ReportChartData> MonthlyTrendChart { get; set; }          // Line
        public List<ReportChartData> StatusDistributionChart { get; set; }    // Pie
        public List<ReportMultiSeriesChartData> DepartmentComparisonChart { get; set; }   // Stacked Bar

        // Multi-series labels
        public List<string> DepartmentComparisonLabels { get; set; }
        public List<string> DepartmentComparisonSeries { get; set; }

        // Filter Dropdowns
        public List<Branch> Branches { get; set; }

        public GraphAnalyticsViewModel()
        {
            EmployeePresentChart = new List<ReportChartData>();
            MonthlyTrendChart = new List<ReportChartData>();
            StatusDistributionChart = new List<ReportChartData>();
            DepartmentComparisonChart = new List<ReportMultiSeriesChartData>();
            DepartmentComparisonLabels = new List<string>();
            DepartmentComparisonSeries = new List<string>();
            Branches = new List<Branch>();
        }
    }

    // =============================================
    // 6. FULL ATTENDANCE REPORT
    // =============================================

    public class FullAttendanceReportViewModel
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int? BranchId { get; set; }
        public int? DepartmentId { get; set; }
        public int? EmployeeId { get; set; }
        public AttendanceStatus? Status { get; set; }

        // KPI Cards
        public int TotalRecords { get; set; }
        public decimal TotalWorkedHours { get; set; }
        public decimal TotalOvertimeHours { get; set; }
        public decimal TotalLessHours { get; set; }

        // Detail Rows
        public List<FullAttendanceRow> Records { get; set; }

        // Filter Dropdowns
        public List<Branch> Branches { get; set; }
        public List<Department> Departments { get; set; }
        public List<Employee> Employees { get; set; }

        public FullAttendanceReportViewModel()
        {
            Records = new List<FullAttendanceRow>();
            Branches = new List<Branch>();
            Departments = new List<Department>();
            Employees = new List<Employee>();
        }
    }

    public class FullAttendanceRow
    {
        public int AttendanceId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string BranchName { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public string ShiftName { get; set; }
        public DateTime AttendanceDate { get; set; }
        public TimeSpan? CheckIn { get; set; }
        public TimeSpan? CheckOut { get; set; }
        public decimal WorkedHours { get; set; }
        public decimal OvertimeHours { get; set; }
        public decimal LessTimeHours { get; set; }
        public int LateMinutes { get; set; }
        public int EarlyGoingMinutes { get; set; }
        public AttendanceStatus Status { get; set; }
        public string StatusText { get; set; }
        public string Remarks { get; set; }
    }
}