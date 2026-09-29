using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TEXCEL_ERP.Models.Entities
{
    /// <summary>
    /// Represents a system setting.
    /// </summary>
    [Table("SystemSettings")]
    public class SystemSetting
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SystemSettingId { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Setting Key")]
        public string SettingKey { get; set; }

        [Required]
        [StringLength(1000)]
        [Display(Name = "Setting Value")]
        public string SettingValue { get; set; }

        [StringLength(500)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [StringLength(50)]
        [Display(Name = "Category")]
        public string Category { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Modified Date")]
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
    }
}