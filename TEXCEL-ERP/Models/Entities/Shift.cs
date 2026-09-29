using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TEXCEL_ERP.Models.Entities
{
    /// <summary>
    /// Represents a work shift.
    /// </summary>
    [Table("Shifts")]
    public class Shift
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ShiftId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Shift Name")]
        public string ShiftName { get; set; }

        [Required]
        [Display(Name = "Start Time")]
        [DataType(DataType.Time)]
        public TimeSpan StartTime { get; set; }

        [Required]
        [Display(Name = "End Time")]
        [DataType(DataType.Time)]
        public TimeSpan EndTime { get; set; }

        [Display(Name = "Break Duration (Minutes)")]
        public int BreakDuration { get; set; } = 60;

        [Display(Name = "Overtime Start After (Hours)")]
        public int OvertimeStartAfter { get; set; } = 8;

        [Display(Name = "Overtime Rate")]
        [Range(1.0, 3.0)]
        public decimal OvertimeRate { get; set; } = 1.5m;

        [Display(Name = "Late Allowance (Minutes)")]
        public int LateAllowance { get; set; } = 15;

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation Properties
        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<Attendance> Attendances { get; set; }

        public Shift()
        {
            Employees = new HashSet<Employee>();
            Attendances = new HashSet<Attendance>();
        }
    }
}