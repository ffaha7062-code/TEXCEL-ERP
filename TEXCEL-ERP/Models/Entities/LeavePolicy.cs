using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TEXCEL_ERP.Models.Enums;

namespace TEXCEL_ERP.Models.Entities
{
    /// <summary>
    /// Represents leave policy configuration.
    /// </summary>
    [Table("LeavePolicies")]
    public class LeavePolicy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LeavePolicyId { get; set; }

        [Required]
        [Display(Name = "Leave Type")]
        public LeaveType LeaveType { get; set; }

        [Required]
        [Display(Name = "Total Leaves Per Year")]
        public int TotalLeaves { get; set; }

        [Display(Name = "Max Carry Forward")]
        public int MaxCarryForward { get; set; } = 0;

        [Display(Name = "Is Encashable")]
        public bool IsEncashable { get; set; } = false;

        [Display(Name = "Requires Attachment")]
        public bool RequiresAttachment { get; set; } = false;

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}