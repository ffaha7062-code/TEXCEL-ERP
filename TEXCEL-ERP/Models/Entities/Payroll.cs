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

        [Display(Name = "Overtime Amount")]
        public decimal OvertimeAmount { get; set; }

        [Display(Name = "Status")]
        public PayrollStatus Status { get; set; } = PayrollStatus.Draft;

        [Display(Name = "Processed Date")]
        public DateTime? ProcessedDate { get; set; }

        [Display(Name = "Processed By")]
        public int? ProcessedById { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation Properties
        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; }

        public virtual ICollection<PayrollDetail> PayrollDetails { get; set; }

        public Payroll()
        {
            PayrollDetails = new HashSet<PayrollDetail>();
        }
    }
}