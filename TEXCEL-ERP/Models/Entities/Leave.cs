using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TEXCEL_ERP.Models.Enums;

namespace TEXCEL_ERP.Models.Entities
{
    /// <summary>
    /// Represents a leave application by an employee.
    /// </summary>
    [Table("Leaves")]
    public class Leave
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LeaveId { get; set; }

        [Required]
        [Display(Name = "Employee")]
        public int EmployeeId { get; set; }

        [Required]
        [Display(Name = "Leave Type")]
        public LeaveType LeaveType { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Display(Name = "Total Days")]
        public int TotalDays { get; set; }

        [Required]
        [StringLength(500)]
        [Display(Name = "Reason")]
        public string Reason { get; set; }

        [Display(Name = "Status")]
        public LeaveStatus Status { get; set; } = LeaveStatus.Pending;

        [Display(Name = "Approved By")]
        public int? ApprovedById { get; set; }

        [Display(Name = "Approved Date")]
        public DateTime? ApprovedDate { get; set; }

        [StringLength(500)]
        [Display(Name = "Approval Remarks")]
        public string ApprovalRemarks { get; set; }

        [StringLength(500)]
        [Display(Name = "Attachment Path")]
        public string AttachmentPath { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation Properties
        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; }
    }
}