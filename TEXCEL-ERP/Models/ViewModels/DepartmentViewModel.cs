using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TEXCEL_ERP.Models.Entities;

namespace TEXCEL_ERP.Models.ViewModels
{
    public class DepartmentViewModel
    {
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "Branch is required")]
        [Display(Name = "Branch")]
        public int BranchId { get; set; }

        [Required(ErrorMessage = "Department Code is required")]
        [StringLength(20)]
        [Display(Name = "Department Code")]
        public string DepartmentCode { get; set; }

        [Required(ErrorMessage = "Department Name is required")]
        [StringLength(100)]
        [Display(Name = "Department Name")]
        public string DepartmentName { get; set; }

        [StringLength(500)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; }
    }
}