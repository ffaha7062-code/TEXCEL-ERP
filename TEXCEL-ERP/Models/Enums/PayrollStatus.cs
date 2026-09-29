using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TEXCEL_ERP.Models.Enums
{
    /// <summary>
    /// Represents the status of payroll processing.
    /// </summary>
    public enum PayrollStatus
    {
        [Display(Name = "Draft")]
        Draft = 1,

        [Display(Name = "Processed")]
        Processed = 2,

        [Display(Name = "Finalized")]
        Finalized = 3,

        [Display(Name = "Reversed")]
        Reversed = 4
    }
}