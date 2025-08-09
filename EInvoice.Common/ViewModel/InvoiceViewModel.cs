using EInvoice.Common.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EInvoice.Common.ViewModel
{
    public class InvoiceViewModel
    {
        [Required]
        public string InvoiceNumber { get; set; }
        [Required]
        public DateTime InvoiceDate { get; set; }
        [Required]
        public long ClientId { get; set; }
        public List<ClientDTO>? Clients { get; set; }
        public List<ProductDTO>? Products { get; set; }
        [MinLength(1, ErrorMessage = "At least one item is required")]
        public List<InvoiceItemDTO> InvoiceItems { get; set; } = new();
    }

}
