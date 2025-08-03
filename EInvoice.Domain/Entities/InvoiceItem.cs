using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EInvoice.Domain.Entities
{
    public class InvoiceItem
    {
        [Key]
        public long Id { get; set; }

        // Link to product (optional - preserves historical data even if product is deleted)
        public long? ProductId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        // Product details (copied from product at time of invoice creation)
        [Required, MaxLength(500)]
        public string ProductDescription { get; set; }

        [Required, MaxLength(20)]
        public string HsCode { get; set; }

        [Required, MaxLength(100)]
        public string UoM { get; set; }

        [Required]
        public decimal Rate { get; set; }

        [Required]
        public int Quantity { get; set; }

        // Calculated values
        [Required]
        public decimal TotalValue { get; set; }

        [Required]
        public decimal ValueSalesExcludingST { get; set; }

        public decimal FixedNotifiedValueOrRetailPrice { get; set; }

        [Required]
        public decimal SalesTaxApplicable { get; set; }

        public decimal SalesTaxWithheldAtSource { get; set; }
        public decimal? ExtraTax { get; set; }
        public decimal? FurtherTax { get; set; }

        [Required]
        public decimal FedPayable { get; set; }

        public decimal Discount { get; set; }

        [Required, MaxLength(200)]
        public string SaleType { get; set; }

        // Relationship
        [Required]
        public long InvoiceId { get; set; }

        [ForeignKey("InvoiceId")]
        public virtual Invoice Invoice { get; set; }
    }
}
