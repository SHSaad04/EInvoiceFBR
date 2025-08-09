using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EInvoice.Common.DTO
{
    public class InvoiceDTO
    {
        public long Id { get; set; }
        [Required]
        public string InvoiceType { get; set; }  // e.g., Sale Invoice, Debit Note
        [Required]
        public DateTime InvoiceDate { get; set; } //2025-04-21

        #region Seller or Organization Details
        public string? SellerNTNCNIC { get; set; }
        public string? SellerBusinessName { get; set; }
        public string? SellerProvince { get; set; }
        public string? SellerAddress { get; set; }
        #endregion

        #region Buyer or Client Details
        public string? BuyerNTNCNIC { get; set; } //Optional in case of Unregistered
        public string? BuyerBusinessName { get; set; }
        public string? BuyerProvince { get; set; }
        public string? BuyerAddress { get; set; }
        public string? BuyerRegistrationType { get; set; }
        #endregion

        [Required]
        public string InvoiceRefNo { get; set; } // Required only in case of debit note
        public string? ScenarioId { get; set; }   // Required for Sandbox only

        #region Relationships
        public long? SellerId { get; set; }
        public OrganizationDTO? Seller { get; set; }
        [Required]
        public long BuyerId { get; set; }
        public ClientDTO? Buyer { get; set; }
        public List<ClientDTO>? Clients { get; set; }
        public List<ProductDTO>? Products { get; set; }
        public List<InvoiceTypeDTO>? InvoiceTypes { get; set; }
        [MinLength(1, ErrorMessage = "At least one item is required")]
        public List<InvoiceItemDTO> InvoiceItems { get; set; } = new();
        #endregion
    }
}
