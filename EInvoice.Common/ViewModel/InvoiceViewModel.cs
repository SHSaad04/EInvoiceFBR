using EInvoice.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EInvoice.Common.ViewModel
{
    public class InvoiceViewModel
    {
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public long ClientId { get; set; }

        public List<ClientDTO> Clients { get; set; }
        public List<ProductDTO> Products { get; set; }

        public List<InvoiceItemDTO> InvoiceItems { get; set; } = new();
    }

}
