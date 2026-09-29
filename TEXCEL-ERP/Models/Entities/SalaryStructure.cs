using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TEXCEL_ERP.Models.Entities
{
    /// <summary>
    /// Represents the salary structure assigned to an employee.
    /// </summary>
    [Table("SalaryStructures")]
    public class SalaryStructure
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SalaryStructureId { get; set; }

        [Required]
        [Display(Name = "Employee")]
        public int EmployeeId { get; set; }

        [Required]
        [Display(Name = "Salary Head")]
        public int SalaryHeadId { get; set; }

        [Required]
        [Display(Name = "Amount")]
        [Range(0, double.MaxValue)]
        public decimal Amount { get; set; }

        [Display(Name = "Percentage")]
        [Range(0, 100)]
        public decimal? Percentage { get; set; }

        [Display(Name = "Effective From")]
        [DataType(DataType.Date)]
        public DateTime EffectiveFrom { get; set; } = DateTime.Now;

        [Display(Name = "Effective To")]
        [DataType(DataType.Date)]
        public DateTime? EffectiveTo { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation Properties
        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; }

        [ForeignKey("SalaryHeadId")]
        public virtual SalaryHead SalaryHead { get; set; }
    }
}