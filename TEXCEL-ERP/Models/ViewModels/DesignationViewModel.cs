using System;
using System.ComponentModel.DataAnnotations;

namespace TEXCEL_ERP.Models.ViewModels
{
    /// <summary>
    /// ViewModel for Designation.
    /// </summary>
    public class DesignationViewModel
    {
        public int DesignationId { get; set; }

        [Required(ErrorMessage = "Department is required")]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "Designation Code is required")]
        [StringLength(20)]
        [Display(Name = "Designation Code")]
        public string DesignationCode { get; set; }

        [Required(ErrorMessage = "Designation Name is required")]
        [StringLength(100)]
        [Display(Name = "Designation Name")]
        public string DesignationName { get; set; }

        [StringLength(10)]
        [Display(Name = "Grade")]
        public string Grade { get; set; }

        [StringLength(500)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; }
    }
}