using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TEXCEL_ERP.Models.Enums;

namespace TEXCEL_ERP.Models.Entities
{
    /// <summary>
    /// Represents daily attendance of an employee.
    /// </summary>
    [Table("Attendances")]
    public class Attendance
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AttendanceId { get; set; }

        [Required]
        [Display(Name = "Employee")]
        public int EmployeeId { get; set; }

        // ✅ NEW: BranchId
        [Required]
        [Display(Name = "Branch")]
        public int BranchId { get; set; }

        [Required]
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public DateTime AttendanceDate { get; set; }

        [Display(Name = "Shift")]
        public int? ShiftId { get; set; }

        [Display(Name = "Check In")]
        [DataType(DataType.Time)]
        public TimeSpan? CheckIn { get; set; }

        [Display(Name = "Check Out")]
        [DataType(DataType.Time)]
        public TimeSpan? CheckOut { get; set; }

        [Display(Name = "Status")]
        public AttendanceStatus Status { get; set; }

        [Display(Name = "Late Minutes")]
        public int LateMinutes { get; set; } = 0;

        [Display(Name = "Early Going Minutes")]
        public int EarlyGoingMinutes { get; set; } = 0;

        [Display(Name = "Overtime Hours")]
        public decimal OvertimeHours { get; set; } = 0;

        // ✅ NEW: Less Time Hours
        [Display(Name = "Less Time Hours")]
        public decimal LessTimeHours { get; set; } = 0;

        [Display(Name = "Worked Hours")]
        public decimal WorkedHours { get; set; } = 0;

        [StringLength(500)]
        [Display(Name = "Remarks")]
        public string Remarks { get; set; }

        [Display(Name = "Is Approved")]
        public bool IsApproved { get; set; } = false;

        // ✅ NEW: Verification / Sign
        [Display(Name = "Is Verified")]
        public bool IsVerified { get; set; } = false;

        [Display(Name = "Verified By")]
        public int? VerifiedBy { get; set; }

        [Display(Name = "Verified Date")]
        public DateTime? VerifiedDate { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; }

        // Navigation Properties
        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; }

        // ✅ NEW: Branch navigation
        [ForeignKey("BranchId")]
        public virtual Branch Branch { get; set; }

        [ForeignKey("ShiftId")]
        public virtual Shift Shift { get; set; }
    }
}