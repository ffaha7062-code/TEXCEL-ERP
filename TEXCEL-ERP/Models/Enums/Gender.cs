using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TEXCEL_ERP.Models.Enums
{
    /// <summary>
    /// Represents the gender of an employee.
    /// </summary>
    public enum Gender
    {
        [Display(Name = "Male")]
        Male = 1,

        [Display(Name = "Female")]
        Female = 2,

        [Display(Name = "Other")]
        Other = 3
    }
}