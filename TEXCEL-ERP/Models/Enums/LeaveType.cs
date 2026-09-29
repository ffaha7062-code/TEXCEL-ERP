using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TEXCEL_ERP.Models.Enums
{
    /// <summary>
    /// Represents the type of leave.
    /// </summary>
    public enum LeaveType
    {
        [Display(Name = "Annual Leave")]
        Annual = 1,

        [Display(Name = "Sick Leave")]
        Sick = 2,

        [Display(Name = "Casual Leave")]
        Casual = 3,

        [Display(Name = "Maternity Leave")]
        Maternity = 4,

        [Display(Name = "Paternity Leave")]
        Paternity = 5,

        [Display(Name = "Unpaid Leave")]
        Unpaid = 6,

        [Display(Name = "Compensatory Leave")]
        Compensatory = 7,

        [Display(Name = "Marriage Leave")]
        Marriage = 8,

        [Display(Name = "Bereavement Leave")]
        Bereavement = 9,

        [Display(Name = "Study Leave")]
        Study = 10,

        [Display(Name = "Hajj/Umrah Leave")]
        HajjUmrah = 11
    }
}