using System.ComponentModel.DataAnnotations;

namespace EInvoice.Common.Entities
{
    public class UserDTO
    {
        public string? Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required] 
        public string LastName { get; set; }
        [Required] 
        public string UserName { get; set; }
        [Required, EmailAddress] 
        public string Email { get; set; }
        [Required, DataType(DataType.Password)] 
        public string Password { get; set; }
        public string Contact { set; get; }
        public string Country { set; get; }
        public string City { set; get; }
        public string PromoCode { get; set; }
        public long? OrganizationId { get; set; }
    }
}
