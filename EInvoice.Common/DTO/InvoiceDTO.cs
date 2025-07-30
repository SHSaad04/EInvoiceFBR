using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EInvoice.Common.Entities
{
    public class InvoiceDTO
    {
        [Key]
        public int Id { get; set; }

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
        public int SellerId { get; set; }
        public OrganizationDTO Seller { get; set; }

        [Required]
        public int BuyerId { get; set; }
        public ClientDTO Buyer { get; set; }

        public ICollection<InvoiceItemDTO> Items { get; set; }
    }
}
