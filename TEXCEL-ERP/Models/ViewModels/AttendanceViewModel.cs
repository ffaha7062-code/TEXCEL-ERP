using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TEXCEL_ERP.Models.Entities;
using TEXCEL_ERP.Models.Enums;

namespace TEXCEL_ERP.Models.ViewModels
{
    /// <summary>
    /// ViewModel for attendance management and reporting.
    /// </summary>
    public class AttendanceViewModel
    {
        public int AttendanceId { get; set; }

        [Required]
        [Display(Name = "Employee")]
        public int EmployeeId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date")]
        public DateTime AttendanceDate { get; set; }

        [Display(Name = "Shift")]
        public int? ShiftId { get; set; }

        [DataType(DataType.Time)]
        [Display(Name = "Check In")]
        public TimeSpan? CheckIn { get; set; }

        [DataType(DataType.Time)]
        [Display(Name = "Check Out")]
        public TimeSpan? CheckOut { get; set; }

        [Display(Name = "Status")]
        public AttendanceStatus Status { get; set; }

        [Display(Name = "Late Minutes")]
        public int LateMinutes { get; set; }

        [Display(Name = "Overtime Hours")]
        public decimal OvertimeHours { get; set; }

        [StringLength(500)]
        [Display(Name = "Remarks")]
        public string Remarks { get; set; }

        // For Reports
        public string ReportType { get; set; }
        public DateTime? ReportDate { get; set; }
        public int? ReportMonth { get; set; }
        public int? ReportYear { get; set; }

        // For Filters
        public int? FilterBranchId { get; set; }
        public int? FilterDepartmentId { get; set; }
        public int? FilterDesignationId { get; set; }
        public int? FilterEmployeeId { get; set; }
        public DateTime? FilterFromDate { get; set; }
        public DateTime? FilterToDate { get; set; }
        public AttendanceStatus? FilterStatus { get; set; }

        // For Dropdowns
        public List<Employee> Employees { get; set; }
        public List<Shift> Shifts { get; set; }
        public List<Branch> Branches { get; set; }
        public List<Department> Departments { get; set; }
        public List<Designation> Designations { get; set; }

        // For Report Data
        public List<Attendance> AttendanceList { get; set; }
        public int TotalPresent { get; set; }
        public int TotalAbsent { get; set; }
        public int TotalLeave { get; set; }
        public int TotalLate { get; set; }
        public decimal TotalOvertime { get; set; }
    }

    // =============================================
    // BULK ENTRY CLASSES
    // =============================================

    /// <summary>
    /// Row for bulk entry page (Employee + their attendance).
    /// </summary>
    public class EmployeeAttendanceRow
    {
        public int EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public int? ShiftId { get; set; }
        public string ShiftName { get; set; }
        public TimeSpan ShiftStartTime { get; set; }
        public TimeSpan ShiftEndTime { get; set; }
        public int BreakDuration { get; set; }
        public int AttendanceId { get; set; }
        public TimeSpan? CheckIn { get; set; }
        public TimeSpan? CheckOut { get; set; }
        public AttendanceStatus Status { get; set; }
        public decimal WorkedHours { get; set; }
        public decimal OvertimeHours { get; set; }
        public decimal LessTimeHours { get; set; }
        public int LateMinutes { get; set; }
        public int EarlyGoingMinutes { get; set; }
        public string Remarks { get; set; }
        public bool IsVerified { get; set; }
        public bool HasAttendance { get; set; }
    }

    /// <summary>
    /// Input model for bulk attendance save.
    /// </summary>
    public class BulkAttendanceInput
    {
        public int EmployeeId { get; set; }
        public string CheckIn { get; set; }
        public string CheckOut { get; set; }
        public int Status { get; set; }
        public string Remarks { get; set; }
        public bool IsVerified { get; set; }
    }

    // =============================================
    // MONTHLY GRID CLASSES
    // =============================================

    /// <summary>
    /// Monthly grid view model.
    /// </summary>
    public class MonthlyAttendanceGridViewModel
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public string MonthName { get; set; }
        public List<MonthlyGridEmployee> Employees { get; set; }

        public int DaysInMonth
        {
            get { return DateTime.DaysInMonth(Year, Month); }
        }
    }

    public class MonthlyGridEmployee
    {
        public int EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public Dictionary<int, string> DailyStatus { get; set; }
    }
}