namespace EInvoice.Common.DTO
{
    public class AuthenticateRequestDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string? IpAddress { get; set; }
        public string? Device { get; set; }
        public string? Browser { get; set; }
    }
}
