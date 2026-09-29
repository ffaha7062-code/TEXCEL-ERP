using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TEXCEL_ERP.Models.Entities
{
    /// <summary>
    /// Represents a system notification.
    /// </summary>
    [Table("Notifications")]
    public class Notification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NotificationId { get; set; }

        [Display(Name = "User")]
        public int? UserId { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [StringLength(1000)]
        [Display(Name = "Message")]
        public string Message { get; set; }

        [StringLength(50)]
        [Display(Name = "Type")]
        public string Type { get; set; } // Info, Success, Warning, Error

        [StringLength(50)]
        [Display(Name = "Module")]
        public string Module { get; set; }

        [Display(Name = "Reference ID")]
        public int? ReferenceId { get; set; }

        [Display(Name = "Is Read")]
        public bool IsRead { get; set; } = false;

        [Display(Name = "Read Date")]
        public DateTime? ReadDate { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}