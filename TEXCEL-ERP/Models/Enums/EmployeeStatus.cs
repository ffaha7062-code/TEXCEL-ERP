using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TEXCEL_ERP.Models.Enums
{
    /// <summary>
    /// Represents the current employment status of an employee.
    /// </summary>
    public enum EmployeeStatus
    {
        [Display(Name = "Active")]
        Active = 1,

        [Display(Name = "Inactive")]
        Inactive = 2,

        [Display(Name = "Probation")]
        Probation = 3,

        [Display(Name = "Confirmed")]
        Confirmed = 4,

        [Display(Name = "Resigned")]
        Resigned = 5,

        [Display(Name = "Retired")]
        Retired = 6
    }
}