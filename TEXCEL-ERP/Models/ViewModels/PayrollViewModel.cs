using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TEXCEL_ERP.Models.Entities;
using TEXCEL_ERP.Models.Enums;

namespace TEXCEL_ERP.Models.ViewModels
{
    /// <summary>
    /// ViewModel for payroll processing and reporting.
    /// </summary>
    public class PayrollViewModel
    {
        public int PayrollId { get; set; }

        [Required]
        [Display(Name = "Employee")]
        public int EmployeeId { get; set; }

        [Required]
        [Display(Name = "Month")]
        [Range(1, 12)]
        public int Month { get; set; }

        [Required]
        [Display(Name = "Year")]
        [Range(2000, 2100)]
        public int Year { get; set; }

        [Display(Name = "Basic Salary")]
        public decimal BasicSalary { get; set; }

        [Display(Name = "Total Earnings")]
        public decimal TotalEarnings { get; set; }

        [Display(Name = "Total Deductions")]
        public decimal TotalDeductions { get; set; }

        [Display(Name = "Net Salary")]
        public decimal NetSalary { get; set; }

        [Display(Name = "Present Days")]
        public int PresentDays { get; set; }

        [Display(Name = "Absent Days")]
        public int AbsentDays { get; set; }

        [Display(Name = "Leave Days")]
        public int LeaveDays { get; set; }

        [Display(Name = "Overtime Hours")]
        public decimal OvertimeHours { get; set; }

        [Display(Name = "Status")]
        public PayrollStatus Status { get; set; }

        // For Dropdowns
        public List<Employee> Employees { get; set; }
        public List<Branch> Branches { get; set; }
        public List<Department> Departments { get; set; }
        public List<SalaryHead> SalaryHeads { get; set; }

        // For Filters
        public int? FilterBranchId { get; set; }
        public int? FilterDepartmentId { get; set; }
        public int? FilterEmployeeId { get; set; }
        public int? FilterMonth { get; set; }
        public int? FilterYear { get; set; }
        public PayrollStatus? FilterStatus { get; set; }

        // For Report Data
        public List<Payroll> PayrollList { get; set; }
        public decimal TotalGross { get; set; }
        public decimal TotalDeduction { get; set; }
        public decimal TotalNet { get; set; }
    }
}