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
        [Display(Name = "Products")]
        [Required]
        public long Id { get; set; }
        [Required]
        [Display(Name = "Harmonized System (HS) Code")]
        public string HsCode { get; set; }
        [Display(Name = "Details of the product or service sold")]
        public string ProductDescription { get; set; }
        [Required]
        [Display(Name = "Tax Rate")]
        public decimal Rate { get; set; }
        [Required]
        [Display(Name = "Unit of Measurement")]
        public string UoM { get; set; }
        [Required]
        [Display(Name = "Notified fixed price or retail price")]
        public decimal FixedNotifiedValueOrRetailPrice { get; set; }
        public long? OrganizationId { get; set; }
    }
}
