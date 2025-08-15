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
        [Display(Name = "Product")]
        [Required(ErrorMessage = "Product is required")]
        public long Id { get; set; }
        [Required(ErrorMessage = "Product is required")]
        public long ProductId { get; set; }
        [Required(ErrorMessage = "HS HsCode is required")]
        [Display(Name = "Harmonized System (HS) Code of the product")]
        public string HsCode { get; set; }
        [Required(ErrorMessage = "Description is required")]
        [Display(Name = "Product Description")]
        public string ProductDescription { get; set; }
        [Required(ErrorMessage = "Rate is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Rate must be greater than 0")]
        [Display(Name = "Tax Rate")]
        public decimal Rate { get; set; }
        [Required(ErrorMessage = "Unit of Measure is required")]
        [Display(Name = "Unit of Measurement")]
        public string UoM { get; set; }
        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        [Display(Name = "Quantity of the item sold")]
        public decimal Quantity { get; set; }

        [Required(ErrorMessage = "Total value is required")]
        [Display(Name = "Total Sales Value")]
        public decimal TotalValue { get; set; }
        [Required]
        [Display(Name = "Sales Value Excluding sales tax")]
        public decimal ValueSalesExcludingST { get; set; }
        [Required]
        [Display(Name = "Notified fixed price or retail price")]
        public decimal FixedNotifiedValueOrRetailPrice { get; set; }

        [Required(ErrorMessage = "Sales Tax is required")]
        [Display(Name = "Amount of Sales Tax/ FED in sales tax mode (Excluding Further & Extra tax)")]
        public decimal SalesTaxApplicable { get; set; }
        [Required]
        [Display(Name = "Sales Tax Withheld at source")]
        public decimal SalesTaxWithheldAtSource { get; set; }
        [Display(Name = "Any Extra Tax")]
        public decimal? ExtraTax { get; set; }
        [Display(Name = "Any Further Tax")]
        public decimal? FurtherTax { get; set; }
        [Display(Name = "SRO Schedule No")]
        public string? SroScheduleNo { get; set; }
        [Display(Name = "Federal excise duty payable")]
        public decimal? FedPayable { get; set; }
        [Display(Name = "Any Discount")]
        public decimal? Discount { get; set; }
        [Required]
        [Display(Name = "Type of Sale")]
        public string SaleType { get; set; }
        [Display(Name = "Item serial number in SRO")]
        public string? SroItemSerialNo { get; set; }
    }
}
