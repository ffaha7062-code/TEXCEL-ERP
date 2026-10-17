using System;
using System.Collections.Generic;
using TEXCEL_ERP.Models.Entities;
using TEXCEL_ERP.Models.Enums;
using TEXCEL_ERP.Models.ViewModels;

namespace TEXCEL_ERP.Services
{
    /// <summary>
    /// Service interface for Attendance Reports.
    /// </summary>
    public interface IAttendanceReportService
    {
        // 1. Daily Report
        DailyReportViewModel GetDailyReport(DateTime date, int? branchId, int? departmentId);

        // 2. Monthly Summary Report
        MonthlySummaryReportViewModel GetMonthlySummaryReport(int month, int year, int? branchId, int? departmentId);

        // 3. Late & OT Report
        LateOTReportViewModel GetLateOTReport(DateTime fromDate, DateTime toDate, int? branchId, int? departmentId);

        // 4. Department-Wise Report
        DepartmentReportViewModel GetDepartmentReport(DateTime fromDate, DateTime toDate, int? branchId);

        // 5. Graph Analytics
        GraphAnalyticsViewModel GetGraphAnalytics(int month, int year, int? branchId);

        // 6. Full Attendance Report
        FullAttendanceReportViewModel GetFullAttendanceReport(
            DateTime fromDate,
            DateTime toDate,
            int? branchId,
            int? departmentId,
            int? employeeId,
            AttendanceStatus? status);
    }
}