using System.ComponentModel.DataAnnotations;

namespace TEXCEL_ERP.Models.Enums
{
    /// <summary>
    /// Represents the role of a user in the system.
    /// </summary>
    public enum UserRole
    {
        [Display(Name = "Super Admin")]
        SuperAdmin = 1,

        [Display(Name = "Branch Admin")]
        BranchAdmin = 2,

        [Display(Name = "Manager")]
        Manager = 3,

        [Display(Name = "Employee")]
        Employee = 4
    }
}