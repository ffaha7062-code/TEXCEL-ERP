using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TEXCEL_ERP.Models.Entities
{
    /// <summary>
    /// Represents a breakdown of payroll (earning or deduction).
    /// </summary>
    [Table("PayrollDetails")]
    public class PayrollDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PayrollDetailId { get; set; }

        [Required]
        [Display(Name = "Payroll")]
        public int PayrollId { get; set; }

        [Required]
        [Display(Name = "Salary Head")]
        public int SalaryHeadId { get; set; }

        [Required]
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }

        [Display(Name = "Type")]
        [StringLength(20)]
        public string Type { get; set; } // "Earning" or "Deduction"

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation Properties
        [ForeignKey("PayrollId")]
        public virtual Payroll Payroll { get; set; }

        [ForeignKey("SalaryHeadId")]
        public virtual SalaryHead SalaryHead { get; set; }
    }
}