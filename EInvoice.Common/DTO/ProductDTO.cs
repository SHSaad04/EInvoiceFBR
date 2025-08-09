using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EInvoice.Common.DTO
{
    public class ProductDTO
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public string HsCode { get; set; }
        public string productDescription { get; set; }
        [Required]
        public decimal Rate { get; set; }
        [Required]
        public string UoM { get; set; }
        [Required]
        public decimal FixedNotifiedValueOrRetailPrice { get; set; }
        public long? OrganizationId { get; set; }
    }
}
