namespace EInvoice.Common.DTO
{
    public class AuthenticateResponseDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Token { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public IEnumerable<string> Roles { get; set; }

    }
}
