using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TEXCEL_ERP.Models.Enums
{
    public enum PaymentStatus
    {
        [Display(Name = "Pending")]
        Pending = 1,

        [Display(Name = "Partial")]
        Partial = 2,

        [Display(Name = "Paid")]
        Paid = 3,

        [Display(Name = "Overdue")]
        Overdue = 4,

        [Display(Name = "Cancelled")]
        Cancelled = 5
    }
}