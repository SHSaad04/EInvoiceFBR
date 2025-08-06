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
        [Required]
        public string InvoiceType { get; set; }  // e.g., Sale Invoice, Debit Note
        [Required]
        public DateTime InvoiceDate { get; set; } //2025-04-21

        #region Seller or Organization Details
        [Required]
        public string SellerNTNCNIC { get; set; }
        [Required]
        public string SellerBusinessName { get; set; }
        [Required]
        public string SellerProvince { get; set; }
        [Required]
        public string SellerAddress { get; set; }
        #endregion

        #region Buyer or Client Details
        public string? BuyerNTNCNIC { get; set; } //Optional in case of Unregistered
        [Required]
        public string BuyerBusinessName { get; set; }
        [Required]
        public string BuyerProvince { get; set; }
        [Required]
        public string BuyerAddress { get; set; }
        [Required]
        public string BuyerRegistrationType { get; set; }
        #endregion

        public string? InvoiceRefNo { get; set; } // Required only in case of debit note
        public string? ScenarioId { get; set; }   // Required for Sandbox only

        #region Relationships
        [Required]
        public long SellerId { get; set; }
        [ForeignKey("SellerId")]
        public virtual Organization Seller { get; set; }
        [Required]
        public long BuyerId { get; set; }
        [ForeignKey("BuyerId")]
        public virtual Client Buyer { get; set; }
        public virtual List<InvoiceItem> InvoiceItems { get; set; }
        #endregion
    }
}
