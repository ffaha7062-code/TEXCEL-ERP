using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TEXCEL_ERP.Models.Entities
{
    /// <summary>
    /// Represents a document uploaded for an employee.
    /// </summary>
    [Table("EmployeeDocuments")]
    public class EmployeeDocument
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployeeDocumentId { get; set; }

        [Required]
        [Display(Name = "Employee")]
        public int EmployeeId { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Document Type")]
        public string DocumentType { get; set; } // CNIC, Degree, Experience Letter, etc.

        [Required]
        [StringLength(200)]
        [Display(Name = "Document Name")]
        public string DocumentName { get; set; }

        [Required]
        [StringLength(500)]
        [Display(Name = "File Path")]
        public string FilePath { get; set; }

        [Display(Name = "File Size (KB)")]
        public long? FileSizeKB { get; set; }

        [StringLength(100)]
        [Display(Name = "Content Type")]
        public string ContentType { get; set; }

        [Display(Name = "Expiry Date")]
        [DataType(DataType.Date)]
        public DateTime? ExpiryDate { get; set; }

        [Display(Name = "Uploaded Date")]
        public DateTime UploadedDate { get; set; } = DateTime.Now;

        [Display(Name = "Uploaded By")]
        public int? UploadedById { get; set; }

        // Navigation Properties
        [ForeignKey("EmployeeId")]
        public virtual Employee Employee { get; set; }
    }
}