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
        [Required, MaxLength(50)]
        public string Name { get; set; }
        [Required, MaxLength(50)]
        public string Code { get; set; }

        [Required, MaxLength(500)]
        public string Description { get; set; }

        [Required, MaxLength(20)]
        public string HsCode { get; set; }

        [Required, MaxLength(100)]
        public string UoM { get; set; } // Unit of Measurement

        [Required]
        public decimal Price { get; set; }

        [MaxLength(50)]
        public string SroScheduleNo { get; set; }

        [MaxLength(50)]
        public string SroItemSerialNo { get; set; }

        // Tax-related properties
        [Required, MaxLength(10)]
        public decimal TaxRate { get; set; } // e.g., "18%"
        public bool IsTaxable { get; set; } = true;
        public bool IsFixedNotified { get; set; }

        // Organization relationship
        [Required]
        public long? OrganizationId { get; set; }

        [ForeignKey("OrganizationId")]
        public virtual Organization Organization { get; set; }
    }
}
