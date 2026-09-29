using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TEXCEL_ERP.Models.Enums
{
    /// <summary>
    /// Represents the status of a sales or purchase order.
    /// </summary>
    public enum OrderStatus
    {
        [Display(Name = "Draft")]
        Draft = 1,

        [Display(Name = "Approved")]
        Approved = 2,

        [Display(Name = "In Production")]
        InProduction = 3,

        [Display(Name = "Dispatched")]
        Dispatched = 4,

        [Display(Name = "Delivered")]
        Delivered = 5,

        [Display(Name = "Closed")]
        Closed = 6,

        [Display(Name = "Cancelled")]
        Cancelled = 7
    }
}