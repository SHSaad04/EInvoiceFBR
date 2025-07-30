using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EInvoice.Domain.Entities
{
    public class Invoice
    {
        [Key]
        public long Id { get; set; }

        [Required, MaxLength(50)]
        public string InvoiceType { get; set; }  // e.g., Sale Invoice, Debit Note

        [Required]
        public DateTime InvoiceDate { get; set; }

        [MaxLength(100)]
        public string InvoiceRefNo { get; set; } // only for Debit Notes

        [MaxLength(50)]
        public string ScenarioId { get; set; }   // Sandbox only

        // Relationships
        [Required]
        public long SellerId { get; set; }
        [ForeignKey("SellerId")]
        public virtual Organization Seller { get; set; }

        [Required]
        public long BuyerId { get; set; }
        [ForeignKey("BuyerId")]
        public virtual Client Buyer { get; set; }
        public virtual List<InvoiceItem> Items { get; set; }
    }
}
