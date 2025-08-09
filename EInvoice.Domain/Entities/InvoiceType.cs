using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EInvoice.Domain.Entities
{
    public class InvoiceType
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public string Type { get; set; }
    }
}
