namespace EInvoice.Common.Exceptions.Types
{
    public class UserTypeException : Exception
    {
        public UserTypeException() { }
        public UserTypeException(string ExceptionMessage) : base(ExceptionMessage) { }
    }
}
