using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TEXCEL_ERP.Models.Enums
{
    /// <summary>
    /// Represents the attendance status of an employee for a specific day.
    /// </summary>
    public enum AttendanceStatus
    {
        [Display(Name = "Present")]
        Present = 1,

        [Display(Name = "Absent")]
        Absent = 2,

        [Display(Name = "Leave")]
        Leave = 3,

        [Display(Name = "Half Day")]
        HalfDay = 4,

        [Display(Name = "Week Off")]
        WeekOff = 5,

        [Display(Name = "Holiday")]
        Holiday = 5,

        [Display(Name = "Weekend")]
        Weekend = 6,

        [Display(Name = "Late")]
        Late = 7,

        [Display(Name = "Early Going")]
        EarlyGoing = 8
    }
}