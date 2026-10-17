using System;
using System.ComponentModel.DataAnnotations;

namespace TEXCEL_ERP.Models.ViewModels
{
    /// <summary>
    /// ViewModel for Shift.
    /// </summary>
    public class ShiftViewModel
    {
        public int ShiftId { get; set; }

        [Required(ErrorMessage = "Branch is required")]
        [Display(Name = "Branch")]
        public int BranchId { get; set; }

        [Required(ErrorMessage = "Shift Name is required")]
        [StringLength(50)]
        [Display(Name = "Shift Name")]
        public string ShiftName { get; set; }

        [Required(ErrorMessage = "Start Time is required")]
        [DataType(DataType.Time)]
        [Display(Name = "Start Time")]
        public TimeSpan StartTime { get; set; }

        [Required(ErrorMessage = "End Time is required")]
        [DataType(DataType.Time)]
        [Display(Name = "End Time")]
        public TimeSpan EndTime { get; set; }

        [Display(Name = "Break Duration (Minutes)")]
        [Range(0, 300)]
        public int BreakDuration { get; set; } = 60;

        [Display(Name = "Shift Type")]
        [StringLength(20)]
        public string ShiftType { get; set; } = "Fixed";

        [Display(Name = "Overtime Rate")]
        [Range(1.0, 3.0)]
        public decimal OvertimeRate { get; set; } = 1.5m;

        [Display(Name = "Late Allowance (Minutes)")]
        [Range(0, 120)]
        public int LateAllowance { get; set; } = 15;

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; }
    }
}