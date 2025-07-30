namespace EInvoice.Common.Exceptions.Types
{
    public class ConfirmationTypeException : Exception
    {
        public ConfirmationType Confirmation { get; set; }
        public ConfirmationTypeException(string ExceptionMessage,
            ConfirmationType Confirmation) : base(ExceptionMessage)
        {
            this.Confirmation = Confirmation;
        }
    }
}
