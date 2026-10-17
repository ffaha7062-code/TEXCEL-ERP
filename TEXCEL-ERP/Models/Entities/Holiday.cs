using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TEXCEL_ERP.Models.Entities
{
    /// <summary>
    /// Represents a holiday (public / branch-specific).
    /// </summary>
    [Table("Holidays")]
    public class Holiday
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int HolidayId { get; set; }

        [Required]
        [Display(Name = "Branch")]
        public int BranchId { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Holiday Name")]
        public string HolidayName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Holiday Date")]
        public DateTime HolidayDate { get; set; }

        [StringLength(500)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Is Paid Holiday")]
        public bool IsPaid { get; set; } = true;

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation Properties
        [ForeignKey("BranchId")]
        public virtual Branch Branch { get; set; }
    }
}