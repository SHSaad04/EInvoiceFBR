using EInvoice.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EInvoice.Common.ViewModel
{
    public class ProductViewModel
    {
        public List<ProductDTO>? Products { get; set; }
        //public ProductDTO? Product { get; set; }
        public InvoiceItemDTO InvoiceItem { get; set; } = new();
    }
}
