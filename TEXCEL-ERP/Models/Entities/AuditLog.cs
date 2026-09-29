using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TEXCEL_ERP.Models.Entities
{
    /// <summary>
    /// Represents an audit log entry for tracking user actions.
    /// </summary>
    [Table("AuditLogs")]
    public class AuditLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AuditLogId { get; set; }

        [Display(Name = "User")]
        public int? UserId { get; set; }

        [StringLength(100)]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Display(Name = "Branch")]
        public int? BranchId { get; set; }

        [StringLength(50)]
        [Display(Name = "Action Type")]
        public string ActionType { get; set; } // Create, Read, Update, Delete, Login, Logout

        [StringLength(100)]
        [Display(Name = "Module")]
        public string Module { get; set; }

        [StringLength(500)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [StringLength(50)]
        [Display(Name = "IP Address")]
        public string IPAddress { get; set; }

        [StringLength(500)]
        [Display(Name = "Old Value")]
        public string OldValue { get; set; }

        [StringLength(500)]
        [Display(Name = "New Value")]
        public string NewValue { get; set; }

        [Display(Name = "Timestamp")]
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}