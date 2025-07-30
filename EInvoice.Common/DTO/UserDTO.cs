namespace EInvoice.Common.Entities
{
    public class UserDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Contact { set; get; }
        public string Country { set; get; }
        public string City { set; get; }
        public string AvatarURL { get; set; }
        public bool? IsAdmin { set; get; }
        public string PromoCode { get; set; }
    }
}
