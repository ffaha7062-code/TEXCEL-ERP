using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TEXCEL_ERP.Models.Entities
{
    /// <summary>
    /// Represents a dispatch of goods against a sales order.
    /// </summary>
    [Table("Dispatches")]
    public class Dispatch
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DispatchId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Dispatch Number")]
        public string DispatchNumber { get; set; }

        [Required]
        [Display(Name = "Sales Order")]
        public int SalesOrderId { get; set; }

        [Required]
        [Display(Name = "Branch")]
        public int BranchId { get; set; }

        [Display(Name = "Dispatch Date")]
        [DataType(DataType.Date)]
        public DateTime DispatchDate { get; set; } = DateTime.Now;

        [Display(Name = "Vehicle Number")]
        [StringLength(50)]
        public string VehicleNumber { get; set; }

        [Display(Name = "Driver Name")]
        [StringLength(100)]
        public string DriverName { get; set; }

        [Display(Name = "Driver Contact")]
        [StringLength(20)]
        public string DriverContact { get; set; }

        [Display(Name = "Expected Delivery Date")]
        [DataType(DataType.Date)]
        public DateTime? ExpectedDeliveryDate { get; set; }

        [Display(Name = "Delivered Date")]
        public DateTime? DeliveredDate { get; set; }

        [Display(Name = "Status")]
        [StringLength(20)]
        public string Status { get; set; } = "Pending"; // Pending, InTransit, Delivered, Cancelled

        [Display(Name = "Total Cartons")]
        public int? TotalCartons { get; set; }

        [Display(Name = "Total Quantity")]
        public decimal TotalQuantity { get; set; }

        [StringLength(500)]
        [Display(Name = "Shipping Address")]
        public string ShippingAddress { get; set; }

        [StringLength(500)]
        [Display(Name = "Remarks")]
        public string Remarks { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation Properties
        [ForeignKey("SalesOrderId")]
        public virtual SalesOrder SalesOrder { get; set; }

        [ForeignKey("BranchId")]
        public virtual Branch Branch { get; set; }
    }
}