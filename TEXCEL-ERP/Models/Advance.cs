using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TEXCEL_ERP.Models.Entities
{
    /// <summary>
    /// Represents a short-term advance taken by an employee.
    /// Advance is recovered in installments from monthly payroll.
    /// </summary>
    [Table("Advances")]
    public class Advance
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AdvanceId { get; set; }

        [Required]
        [Display(Name = "Employee")]
        public int EmployeeId { get; set; }

        [Required]
        [Display(Name = "Advance Amount")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal AdvanceAmount { get; set; }

        [Display(Name = "Recovered Amount")]
        public decimal RecoveredAmount { get; set; } = 0;

        [Display(Name = "Outstanding Amount")]
        public decimal OutstandingAmount { get; set; }

        [Required]
        [Display(Name = "Installments")]
        [Range(1, 60, ErrorMessage = "Installments must be between 1 and 60")]
        public int Installments { get; set; }

        [Display(Name = "Monthly Installment")]
        public decimal MonthlyInstallment { get; set; }

        [Required]
        [Display(Name = "Advance Date")]
        [DataType(DataType.Date)]
        public DateTime AdvanceDate { get; set; } = DateTime.Now;

        [Display(Name = "Start Deduction From")]
        [DataType(DataType.Date)]
        public DateTime? StartDeductionDate { get; set; }

        [StringLength(500)]
        [Display(Name = "Reason")]
        public string Reason { get; set; }

        [Display(Name = "Status")]
        public Enums.AdvanceStatus Status { get; set; } = Enums.AdvanceStatus.Pending;

        [Display(Name = "Approved By")]
        public int? ApprovedById { get; set; }

        [Display(Name = "Approved Date")]
        public DateTime? ApprovedDate { get; set; }

        [Display(Name = "Disbursed By")]
        public int? DisbursedById { get; set; }

        [Display(Name = "Disbursed Date")]
        public DateTime? DisbursedDate { get; set; }

        [StringLength(20)]
        [Display(Name = "Payment Method")]
        public string PaymentMethod { get; set; } // Cash, Bank, Cheque

        [StringLength(500)]
        [Display(Name = "Remarks")]
        public string Remarks { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; }

        // Navigation Properties
        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; }

        public virtual ICollection<AdvanceRecovery> Recoveries { get; set; }

        public Advance()
        {
            Recoveries = new HashSet<AdvanceRecovery>();
        }
    }
}