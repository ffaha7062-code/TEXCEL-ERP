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

        [Display(Name = "Worked Hours")]
        public decimal WorkedHours { get; set; } = 0;

        [StringLength(500)]
        [Display(Name = "Remarks")]
        public string Remarks { get; set; }

        [Display(Name = "Is Approved")]
        public bool IsApproved { get; set; } = false;

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation Properties
        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; }

        [ForeignKey("ShiftId")]
        public virtual Shift Shift { get; set; }
    }
}