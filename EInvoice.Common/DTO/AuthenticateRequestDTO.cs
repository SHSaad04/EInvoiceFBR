using System.ComponentModel.DataAnnotations;

namespace EInvoice.Common.DTO
{
    public class AuthenticateRequestDTO
    {
        [Required]
        public string Username { get; set; }
        [Required, DataType(DataType.Password)] 
        public string Password { get; set; }
    }
}
