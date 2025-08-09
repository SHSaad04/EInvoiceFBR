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
        [Display(Name = "Invoice Type")]
        public string InvoiceType { get; set; }  // e.g., Sale Invoice, Debit Note
        [Required]
        [Display(Name = "Invoice Date")]
        public DateTime InvoiceDate { get; set; } //2025-04-21

        #region Seller or Organization Details
        [Display(Name = "Seller NTN/CNIC")]
        public string? SellerNTNCNIC { get; set; }
        [Display(Name = "Seller Business Name")]
        public string? SellerBusinessName { get; set; }
        [Display(Name = "Seller province")]
        public string? SellerProvince { get; set; }
        [Display(Name = "Seller business Address")]
        public string? SellerAddress { get; set; }
        #endregion

        #region Buyer or Client Details
        [Display(Name = "Buyer NTN/CNIC")]
        public string? BuyerNTNCNIC { get; set; } //Optional in case of Unregistered
        [Display(Name = "Buyer Business Name")]
        public string? BuyerBusinessName { get; set; }
        [Display(Name = "Buyer Province")]
        public string? BuyerProvince { get; set; }
        [Display(Name = "Buyer Address")]
        public string? BuyerAddress { get; set; }
        [Display(Name = "Buyer Registration Type")]
        public string? BuyerRegistrationType { get; set; }
        #endregion

        [Required]
        [Display(Name = "Reference Invoice no for the debit note")]
        public string InvoiceRefNo { get; set; } // Required only in case of debit note
        [Display(Name = "Scenario ID / Number")]
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
