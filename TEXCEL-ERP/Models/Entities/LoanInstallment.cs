using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TEXCEL_ERP.Models.Entities
{
    /// <summary>
    /// Represents a monthly installment of a loan.
    /// </summary>
    [Table("LoanInstallments")]
    public class LoanInstallment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LoanInstallmentId { get; set; }

        [Required]
        [Display(Name = "Loan")]
        public int LoanId { get; set; }

        [Required]
        [Display(Name = "Installment Number")]
        public int InstallmentNumber { get; set; }

        [Required]
        [Display(Name = "Due Date")]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        [Required]
        [Display(Name = "EMI Amount")]
        public decimal EMIAmount { get; set; }

        [Display(Name = "Principal Amount")]
        public decimal PrincipalAmount { get; set; }

        [Display(Name = "Interest Amount")]
        public decimal InterestAmount { get; set; }

        [Display(Name = "Paid Amount")]
        public decimal PaidAmount { get; set; } = 0;

        [Display(Name = "Paid Date")]
        public DateTime? PaidDate { get; set; }

        [Display(Name = "Status")]
        [StringLength(20)]
        public string Status { get; set; } = "Pending"; // Pending, Paid, Overdue

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation Properties
        [ForeignKey("LoanId")]
        public virtual Loan Loan { get; set; }
    }
}