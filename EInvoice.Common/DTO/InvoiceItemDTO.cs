using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EInvoice.Common.DTO
{
    public class InvoiceItemDTO
    {
        [Key]
        public long Id { get; set; }
        [Required(ErrorMessage = "Product is required")]
        public long? ProductId { get; set; }
        [Required(ErrorMessage = "HS HsCode is required")]
        public string HsCode { get; set; }
        [Required(ErrorMessage = "Description is required")]
        public string ProductDescription { get; set; }
        [Required(ErrorMessage = "Rate is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Rate must be greater than 0")]
        public decimal Rate { get; set; }
        [Required(ErrorMessage = "Unit of Measure is required")]
        public string UoM { get; set; }
        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public decimal Quantity { get; set; }

        [Required(ErrorMessage = "Total value is required")]
        public decimal TotalValue { get; set; }
        [Required]
        public decimal ValueSalesExcludingST { get; set; }
        [Required]
        public decimal FixedNotifiedValueOrRetailPrice { get; set; }

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
    }
}
