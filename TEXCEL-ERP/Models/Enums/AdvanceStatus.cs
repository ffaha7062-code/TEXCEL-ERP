using System.ComponentModel.DataAnnotations;

namespace TEXCEL_ERP.Models.Enums
{
    /// <summary>
    /// Represents the status of an employee advance.
    /// </summary>
    public enum AdvanceStatus
    {
        [Display(Name = "Pending")]
        Pending = 1,

        [Display(Name = "Approved")]
        Approved = 2,

        [Display(Name = "Disbursed")]
        Disbursed = 3,

        [Display(Name = "Recovering")]
        Recovering = 4,

        [Display(Name = "Settled")]
        Settled = 5,

        [Display(Name = "Cancelled")]
        Cancelled = 6
    }
}