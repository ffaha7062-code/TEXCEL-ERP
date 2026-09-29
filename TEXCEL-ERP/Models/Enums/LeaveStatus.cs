using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TEXCEL_ERP.Models.Enums
{
    /// <summary>
    /// Represents the status of a leave application.
    /// </summary>
    public enum LeaveStatus
    {
        [Display(Name = "Pending")]
        Pending = 1,

        [Display(Name = "Approved")]
        Approved = 2,

        [Display(Name = "Rejected")]
        Rejected = 3,

        [Display(Name = "Cancelled")]
        Cancelled = 4
    }
}