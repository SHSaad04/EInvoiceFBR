using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EInvoice.Domain.Entities
{
    public class Product
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string HsCode { get; set; }
        public string productDescription { get; set; }
        public string UoM { get; set; } // Unit of Measurement
        public int Quantity { get; set; }
        public decimal TotalValue { get; set; }
        public decimal Price { get; set; }
        public string SroScheduleNo { get; set; }
        public string SroItemSerialNo { get; set; }
        public decimal TaxRate { get; set; } // e.g., "18%"
        public bool IsTaxable { get; set; } = true;
        public bool IsFixedNotified { get; set; }
        public long? OrganizationId { get; set; }
        [ForeignKey("OrganizationId")]
        public virtual Organization Organization { get; set; }
    }
}
