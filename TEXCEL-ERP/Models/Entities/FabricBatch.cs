using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TEXCEL_ERP.Models.Entities
{
    /// <summary>
    /// Represents a batch of fabric received from a supplier.
    /// </summary>
    [Table("FabricBatches")]
    public class FabricBatch
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FabricBatchId { get; set; }

        [Required]
        [Display(Name = "Fabric")]
        public int FabricId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Batch Number")]
        public string BatchNumber { get; set; }

        [Display(Name = "Supplier")]
        public int? SupplierId { get; set; }

        [Display(Name = "Purchase Date")]
        [DataType(DataType.Date)]
        public DateTime? PurchaseDate { get; set; }

        [Display(Name = "Expiry Date")]
        [DataType(DataType.Date)]
        public DateTime? ExpiryDate { get; set; }

        [Display(Name = "Dye Lot Number")]
        [StringLength(50)]
        public string DyeLotNumber { get; set; }

        [StringLength(500)]
        [Display(Name = "Quality Certificate Path")]
        public string QualityCertificatePath { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation Properties
        [ForeignKey("FabricId")]
        public virtual Fabric Fabric { get; set; }

        [ForeignKey("SupplierId")]
        public virtual Supplier Supplier { get; set; }
    }
}