using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EInvoice.Domain.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Contact { set; get; }
        public string? Country { set; get; }
        public string? City { set; get; }
        public string? AvatarURL { get; set; }
        public bool? IsAdmin { set; get; }
        public string? PromoCode { get; set; }
        // Relationship: each user belongs to one organization
        public long? OrganizationId { get; set; }

        [ForeignKey("OrganizationId")]
        public virtual Organization Organization { get; set; }
    }
}
