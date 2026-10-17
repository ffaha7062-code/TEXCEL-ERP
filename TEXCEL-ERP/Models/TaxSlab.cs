using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TEXCEL_ERP.Models.Entities
{
    /// <summary>
    /// Represents a tax slab for income tax calculation (FBR Pakistan).
    /// Income is annual.
    /// </summary>
    [Table("TaxSlabs")]
    public class TaxSlab
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TaxSlabId { get; set; }

        [Required]
        [Display(Name = "Year")]
        [Range(2000, 2100)]
        public int Year { get; set; }

        [Required]
        [Display(Name = "Min Income (Annual)")]
        [Range(0, double.MaxValue)]
        public decimal MinIncome { get; set; }

        [Display(Name = "Max Income (Annual)")]
        [Range(0, double.MaxValue)]
        public decimal MaxIncome { get; set; } // 0 = no limit (infinity)

        [Display(Name = "Fixed Tax")]
        public decimal FixedTax { get; set; } = 0;

        [Display(Name = "Percentage (%)")]
        [Range(0, 100)]
        public decimal Percentage { get; set; } = 0;

        [StringLength(200)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}