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
        [Required]
        public string HsCode { get; set; }
        [Required]
        public string productDescription { get; set; }
        [Required]
        public string Rate { get; set; }
        [Required]
        public string UoM { get; set; }
        [Required]
        public Decimal Quantity { get; set; }
        [Required]
        public decimal TotalValues { get; set; }
        [Required]
        public decimal ValueSalesExcludingST { get; set; }
        [Required]
        public decimal FixedNotifiedValueOrRetailPrice { get; set; }
        [Required]
        public decimal SalesTaxApplicable { get; set; }
        [Required]
        public decimal SalesTaxWithheldAtSource { get; set; }
        public decimal? ExtraTax { get; set; }
        public decimal? FurtherTax { get; set; }
        public string? sroScheduleNo { get; set; }
        public decimal? FedPayable { get; set; }
        public decimal? Discount { get; set; }
        [Required]
        public string SaleType { get; set; }
        public string? sroItemSerialNo { get; set; }

        #region Relationship
        [Required]
        public long InvoiceId { get; set; }
        [ForeignKey("InvoiceId")]
        public virtual Invoice Invoice { get; set; }
        // Link to product (optional - preserves historical data even if product is deleted)
        public long? ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        // Product details (copied from product at time of invoice creation)
        #endregion
    }
}
