using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EInvoice.Common.DTO
{
    public class ProductDTO
    {
        [Key]
        public long Id { get; set; }
        [Required]
        [Display(Name = "Harmonized System (HS) Code\r\nof the product. ")]
        public string HsCode { get; set; }
        [Display(Name = "Details of the product or\r\nservice sold. \r\n")]
        public string ProductDescription { get; set; }
        [Required]
        [Display(Name = "Tax Rate")]
        public decimal Rate { get; set; }
        [Required]
        [Display(Name = "Unit of Measurement")]
        public string UoM { get; set; }
        [Required]
        [Display(Name = "Notified fixed price or retail\r\nprice")]
        public decimal FixedNotifiedValueOrRetailPrice { get; set; }
        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public decimal Quantity { get; set; }

        [Required(ErrorMessage = "Total value is required")]
        public decimal TotalValue { get; set; }
        [Required]
        public decimal ValueSalesExcludingST { get; set; }
        [Required(ErrorMessage = "Sales Tax is required")]
        public decimal SalesTaxApplicable { get; set; }
        [Required]
        public decimal SalesTaxWithheldAtSource { get; set; }
        public decimal? ExtraTax { get; set; }
        public decimal? FurtherTax { get; set; }
        public string? SroScheduleNo { get; set; }
        public decimal? FedPayable { get; set; }
        public decimal? Discount { get; set; }
        [Required]
        public string SaleType { get; set; }
        public string? SroItemSerialNo { get; set; }
        public long? OrganizationId { get; set; }
    }
}
