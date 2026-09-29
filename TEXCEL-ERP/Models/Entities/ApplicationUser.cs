using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TEXCEL_ERP.Models.Entities
{
    /// <summary>
    /// Extended Identity User with additional fields.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(100)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [StringLength(100)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Branch")]
        public int? BranchId { get; set; }

        [Display(Name = "Employee")]
        public int? EmployeeId { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Last Login")]
        public DateTime? LastLogin { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [StringLength(500)]
        [Display(Name = "Profile Picture")]
        public string ProfilePicture { get; set; }

        // Navigation Properties
        [ForeignKey("BranchId")]
        public virtual Branch Branch { get; set; }

        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; }

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}".Trim();

        /// <summary>
        /// Generate user identity with custom claims.
        /// </summary>
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            // Add custom claims
            userIdentity.AddClaim(new Claim("FullName", FullName ?? ""));
            userIdentity.AddClaim(new Claim("BranchId", BranchId?.ToString() ?? ""));
            userIdentity.AddClaim(new Claim("EmployeeId", EmployeeId?.ToString() ?? ""));

            return userIdentity;
        }
    }
}