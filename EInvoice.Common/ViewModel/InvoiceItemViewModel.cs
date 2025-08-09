using EInvoice.Common.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EInvoice.Common.ViewModel
{
    public class InvoiceItemViewModel
    {
        [Key]
        public long Id { get; set; }

        [Required, MaxLength(20)]
        public string HsCode { get; set; }

        [Required(ErrorMessage = "Product is required")]
        public long? ProductId { get; set; }
        [Required, StringLength(500)]
        public string ProductDescription { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Rate must be greater than 0")]
        public string Rate { get; set; } // e.g., "18%"

        [Required, MaxLength(100)]
        public string UoM { get; set; } // Unit of Measurement

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

        [Required]
        public double TotalValues { get; set; }

        [Required]
        public double ValueSalesExcludingST { get; set; }

        public double FixedNotifiedValueOrRetailPrice { get; set; }
        [Required]
        public double SalesTaxApplicable { get; set; }

        public double SalesTaxWithheldAtSource { get; set; }
        public double? ExtraTax { get; set; }
        public double? FurtherTax { get; set; }

        [MaxLength(50)]
        public string SroScheduleNo { get; set; }

        public double FedPayable { get; set; }
        public double Discount { get; set; }

        [Required, MaxLength(200)]
        public string SaleType { get; set; }

        [MaxLength(50)]
        public string SroItemSerialNo { get; set; }

        // Relationship
        [Required]
        public long InvoiceId { get; set; }
        public InvoiceDTO Invoice { get; set; }

    }
}
