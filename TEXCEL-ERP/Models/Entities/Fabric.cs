using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TEXCEL_ERP.Models.Enums;

namespace TEXCEL_ERP.Models.Entities
{
    /// <summary>
    /// Represents a fabric type.
    /// </summary>
    [Table("Fabrics")]
    public class Fabric
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FabricId { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Fabric Code")]
        public string FabricCode { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Fabric Type")]
        public string FabricType { get; set; }

        [StringLength(50)]
        [Display(Name = "Composition")]
        public string Composition { get; set; }

        [StringLength(50)]
        [Display(Name = "Weave")]
        public string Weave { get; set; }

        [Display(Name = "GSM")]
        public int? GSM { get; set; }

        [Display(Name = "Width (Inches)")]
        public decimal? Width { get; set; }

        [StringLength(50)]
        [Display(Name = "Color")]
        public string Color { get; set; }

        [StringLength(50)]
        [Display(Name = "Shade Code")]
        public string ShadeCode { get; set; }

        [StringLength(50)]
        [Display(Name = "Design")]
        public string Design { get; set; }

        [Display(Name = "Grade")]
        public FabricGrade Grade { get; set; } = FabricGrade.A;

        [Display(Name = "Unit of Measurement")]
        [StringLength(20)]
        public string UOM { get; set; } = "Meter";

        [Display(Name = "Rate Per Unit")]
        public decimal RatePerUnit { get; set; }

        [Display(Name = "Minimum Stock Level")]
        public decimal MinimumStock { get; set; } = 0;

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation Properties
        public virtual ICollection<FabricBatch> Batches { get; set; }
        public virtual ICollection<FabricStock> Stocks { get; set; }
        public virtual ICollection<GRN> GRNs { get; set; }
        public virtual ICollection<StockIssue> StockIssues { get; set; }

        public Fabric()
        {
            Batches = new HashSet<FabricBatch>();
            Stocks = new HashSet<FabricStock>();
            GRNs = new HashSet<GRN>();
            StockIssues = new HashSet<StockIssue>();
        }
    }
}