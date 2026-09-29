using System;
using System.ComponentModel.DataAnnotations;

namespace TEXCEL_ERP.Models.ViewModels
{
    /// <summary>
    /// ViewModel for Company settings.
    /// </summary>
    public class CompanyViewModel
    {
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "Company Name is required")]
        [StringLength(200)]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [StringLength(100)]
        [Display(Name = "Legal Name")]
        public string LegalName { get; set; }

        [StringLength(50)]
        [Display(Name = "NTN")]
        public string NTN { get; set; }

        [StringLength(50)]
        [Display(Name = "GST Number")]
        public string GSTNumber { get; set; }

        [StringLength(500)]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [StringLength(100)]
        [Display(Name = "City")]
        public string City { get; set; }

        [StringLength(100)]
        [Display(Name = "Country")]
        public string Country { get; set; }

        [StringLength(20)]
        [Display(Name = "Phone")]
        public string Phone { get; set; }

        [StringLength(100)]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [StringLength(200)]
        [Display(Name = "Website")]
        public string Website { get; set; }

        [Display(Name = "Logo")]
        public string LogoPath { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;
    }
}