using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TEXCEL_ERP.Models.Entities
{
    /// <summary>
    /// Represents a single recovery of an advance (from payroll or manual).
    /// </summary>
    [Table("AdvanceRecoveries")]
    public class AdvanceRecovery
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AdvanceRecoveryId { get; set; }

        [Required]
        [Display(Name = "Advance")]
        public int AdvanceId { get; set; }

        [Display(Name = "Payroll")]
        public int? PayrollId { get; set; }

        [Required]
        [Display(Name = "Recovery Date")]
        [DataType(DataType.Date)]
        public DateTime RecoveryDate { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Amount")]
        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }

        [StringLength(500)]
        [Display(Name = "Remarks")]
        public string Remarks { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation Properties
        [ForeignKey("AdvanceId")]
        public virtual Advance Advance { get; set; }

        [ForeignKey("PayrollId")]
        public virtual Payroll Payroll { get; set; }
    }
}