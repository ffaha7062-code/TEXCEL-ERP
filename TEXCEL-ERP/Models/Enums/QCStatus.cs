using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TEXCEL_ERP.Models.Enums
{
    /// <summary>
    /// Represents the quality control inspection result.
    /// </summary>
    public enum QCStatus
    {
        [Display(Name = "Pass")]
        Pass = 1,

        [Display(Name = "Fail")]
        Fail = 2,

        [Display(Name = "Rework")]
        Rework = 3,

        [Display(Name = "Pending")]
        Pending = 4
    }
}