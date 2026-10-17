using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TEXCEL_ERP.Models.Enums;

namespace TEXCEL_ERP.Models.Entities
{
    /// <summary>
    /// Represents monthly payroll of an employee.
    /// </summary>
    [Table("Payrolls")]
    public class Payroll
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PayrollId { get; set; }

        [Required]
        [Display(Name = "Employee")]
        public int EmployeeId { get; set; }

        // =============================================
        // PERIOD
        // =============================================
        [Required]
        [Display(Name = "Month")]
        [Range(1, 12)]
        public int Month { get; set; }

        [Required]
        [Display(Name = "Year")]
        [Range(2000, 2100)]
        public int Year { get; set; }

        // =============================================
        // BRANCH / DEPARTMENT (for reports)
        // =============================================
        [Display(Name = "Branch")]
        public int? BranchId { get; set; }

        [Display(Name = "Department")]
        public int? DepartmentId { get; set; }

        // =============================================
        // SALARY COMPONENTS
        // =============================================
        [Display(Name = "Basic Salary")]
        public decimal BasicSalary { get; set; }

        [Display(Name = "Gross Salary")]
        public decimal GrossSalary { get; set; }

        [Display(Name = "Total Earnings")]
        public decimal TotalEarnings { get; set; }

        [Display(Name = "Total Deductions")]
        public decimal TotalDeductions { get; set; }

        [Display(Name = "Net Salary")]
        public decimal NetSalary { get; set; }

        // =============================================
        // ATTENDANCE SUMMARY
        // =============================================
        [Display(Name = "Total Working Days")]
        public int TotalWorkingDays { get; set; }

        [Display(Name = "Present Days")]
        public int PresentDays { get; set; }

        [Display(Name = "Absent Days")]
        public int AbsentDays { get; set; }

        [Display(Name = "Leave Days")]
        public int LeaveDays { get; set; }

        [Display(Name = "Half Days")]
        public int HalfDays { get; set; }

        [Display(Name = "Holidays")]
        public int Holidays { get; set; }

        [Display(Name = "Total Worked Hours")]
        public decimal TotalWorkedHours { get; set; }

        // =============================================
        // OVERTIME / LATE / LESS TIME
        // =============================================
        [Display(Name = "Overtime Hours")]
        public decimal OvertimeHours { get; set; }

        [Display(Name = "Overtime Amount")]
        public decimal OvertimeAmount { get; set; }

        [Display(Name = "Late Minutes")]
        public int LateMinutes { get; set; }

        [Display(Name = "Late Deduction")]
        public decimal LateDeduction { get; set; }

        [Display(Name = "Less Hours")]
        public decimal LessHours { get; set; }

        [Display(Name = "Less Time Deduction")]
        public decimal LessTimeDeduction { get; set; }

        [Display(Name = "Absent Deduction")]
        public decimal AbsentDeduction { get; set; }

        // =============================================
        // STATUS & WORKFLOW
        // =============================================
        [Display(Name = "Status")]
        public PayrollStatus Status { get; set; } = PayrollStatus.Draft;

        [Display(Name = "Processed Date")]
        public DateTime? ProcessedDate { get; set; }

        [Display(Name = "Processed By")]
        public int? ProcessedById { get; set; }

        [Display(Name = "Approved Date")]
        public DateTime? ApprovedDate { get; set; }

        [Display(Name = "Approved By")]
        public int? ApprovedById { get; set; }

        [Display(Name = "Paid Date")]
        public DateTime? PaidDate { get; set; }

        [StringLength(20)]
        [Display(Name = "Payment Method")]
        public string PaymentMethod { get; set; } // Cash, Bank, Cheque

        // =============================================
        // AUDIT
        // =============================================
        [StringLength(500)]
        [Display(Name = "Remarks")]
        public string Remarks { get; set; }

        [Display(Name = "Created By")]
        public int? CreatedById { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; }

        // =============================================
        // NAVIGATION PROPERTIES
        // =============================================
        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; }

        [ForeignKey("BranchId")]
        public virtual Branch Branch { get; set; }

        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }

        public virtual ICollection<PayrollDetail> PayrollDetails { get; set; }
        public virtual ICollection<AdvanceRecovery> AdvanceRecoveries { get; set; }

        public Payroll()
        {
            PayrollDetails = new HashSet<PayrollDetail>();
            AdvanceRecoveries = new HashSet<AdvanceRecovery>();
        }
    }
}