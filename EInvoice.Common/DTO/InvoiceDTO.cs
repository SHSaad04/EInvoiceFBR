using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EInvoice.Common.Entities
{
    public class InvoiceDTO
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
        public OrganizationDTO Seller { get; set; }

        [Required]
        public long BuyerId { get; set; }
        public ClientDTO Buyer { get; set; }
        public List<InvoiceItemDTO> Items { get; set; }
    }
}
