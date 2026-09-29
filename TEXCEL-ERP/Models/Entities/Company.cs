using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TEXCEL_ERP.Models.Entities
{
    /// <summary>
    /// Represents a company in the ERP system.
    /// </summary>
    [Table("Companies")]
    public class Company
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CompanyId { get; set; }

        [Required]
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

        [StringLength(500)]
        [Display(Name = "Logo Path")]
        public string LogoPath { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; }

        // Navigation Properties
        public virtual ICollection<Branch> Branches { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }

        public Company()
        {
            Branches = new HashSet<Branch>();
            Employees = new HashSet<Employee>();
        }
    }
}