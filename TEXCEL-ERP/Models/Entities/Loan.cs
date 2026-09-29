using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TEXCEL_ERP.Models.Entities
{
    /// <summary>
    /// Represents a loan taken by an employee.
    /// </summary>
    [Table("Loans")]
    public class Loan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LoanId { get; set; }

        [Required]
        [Display(Name = "Employee")]
        public int EmployeeId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Loan Type")]
        public string LoanType { get; set; }

        [Required]
        [Display(Name = "Loan Amount")]
        [Range(0, double.MaxValue)]
        public decimal LoanAmount { get; set; }

        [Display(Name = "Interest Rate (%)")]
        [Range(0, 100)]
        public decimal InterestRate { get; set; } = 0;

        [Display(Name = "Tenure (Months)")]
        [Range(1, 120)]
        public int TenureMonths { get; set; }

        [Display(Name = "EMI Amount")]
        public decimal EMIAmount { get; set; }

        [Display(Name = "Total Payable")]
        public decimal TotalPayable { get; set; }

        [Display(Name = "Total Paid")]
        public decimal TotalPaid { get; set; } = 0;

        [Display(Name = "Outstanding Amount")]
        public decimal OutstandingAmount { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        [StringLength(500)]
        [Display(Name = "Reason")]
        public string Reason { get; set; }

        [Display(Name = "Status")]
        [StringLength(20)]
        public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected, Active, Closed

        [Display(Name = "Approved By")]
        public int? ApprovedById { get; set; }

        [Display(Name = "Approved Date")]
        public DateTime? ApprovedDate { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation Properties
        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; }

        public virtual ICollection<LoanInstallment> Installments { get; set; }

        public Loan()
        {
            Installments = new HashSet<LoanInstallment>();
        }
    }
}