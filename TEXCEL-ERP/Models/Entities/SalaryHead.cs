using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TEXCEL_ERP.Models.Entities
{
    /// <summary>
    /// Represents a salary component (earning or deduction).
    /// </summary>
    [Table("SalaryHeads")]
    public class SalaryHead
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SalaryHeadId { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Head Name")]
        public string HeadName { get; set; }

        [Required]
        [Display(Name = "Head Type")]
        [StringLength(20)]
        public string HeadType { get; set; } // "Earning" or "Deduction"

        [Display(Name = "Calculation Type")]
        [StringLength(50)]
        public string CalculationType { get; set; } // "Fixed", "PercentageOfBasic", "PercentageOfGross"

        [Display(Name = "Default Value")]
        public decimal DefaultValue { get; set; } = 0;

        [Display(Name = "Is Taxable")]
        public bool IsTaxable { get; set; } = true;

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        // ✅ NEW FIELDS
        [Display(Name = "Is System Head")]
        public bool IsSystemHead { get; set; } = false;

        [Display(Name = "Is Statutory")]
        public bool IsStatutory { get; set; } = false;

        [Display(Name = "Is Deduction")]
        public bool IsDeduction { get; set; } = false;

        [Display(Name = "Display Order")]
        public int DisplayOrder { get; set; } = 0;

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation Properties
        public virtual ICollection<SalaryStructure> SalaryStructures { get; set; }
        public virtual ICollection<PayrollDetail> PayrollDetails { get; set; }

        public SalaryHead()
        {
            SalaryStructures = new HashSet<SalaryStructure>();
            PayrollDetails = new HashSet<PayrollDetail>();
        }
    }
}