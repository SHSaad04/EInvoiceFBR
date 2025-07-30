using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EInvoice.Domain.Entities
{
    public class Organization
    {
        [Key]
        public long Id { get; set; }

        [Required, MaxLength(20)]
        public string NTNCNIC { get; set; }

        [Required, MaxLength(200)]
        public string BusinessName { get; set; }

        [Required, MaxLength(100)]
        public string Province { get; set; }

        [Required, MaxLength(300)]
        public string Address { get; set; }

        // Reverse navigation
        public virtual List<Invoice> Invoices { get; set; }
    }
}
