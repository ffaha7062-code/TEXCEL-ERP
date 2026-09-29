using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TEXCEL_ERP.Models.Enums
{
    /// <summary>
    /// Represents the quality grade of fabric.
    /// </summary>
    public enum FabricGrade
    {
        [Display(Name = "Grade A (Prime)")]
        A = 1,

        [Display(Name = "Grade B (Seconds)")]
        B = 2,

        [Display(Name = "Grade C (Reject)")]
        C = 3,

        [Display(Name = "Reject")]
        Reject = 4
    }
}