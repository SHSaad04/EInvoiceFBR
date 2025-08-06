using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EInvoice.Common.Entities
{
    public class InvoiceTypeDTO
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public string Type { get; set; }

    }
}
