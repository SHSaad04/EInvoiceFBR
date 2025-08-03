namespace EInvoice.Common.Exceptions
{
    public class ExceptionMessages
    {
        public static readonly string DataNotFoundExceptionMessage = "Data Not Found.";
        public static readonly string ClientAlreadyExist = "Client with the same NTNCNIC already exists.";
        public static readonly string OrganizationAlreadyExist = "Organization with the same NTNCNIC already exists.";
        public static readonly string RecordAlreadyExist = "A record with the same title already exists.";
        public static readonly string BadRequestExceptionMessage = "Bad Request Response.";
        public static readonly string EmailArleadyInUse = "Email is already in use.";
        public static readonly string PageAlreadyExist = "Page with same name already exists.";
        public static readonly string QuestionAlreadyExist = "Question with same text already exists.";
        public static readonly string OptionAlreadyExist = "Option with same text already exists.";
        public static readonly string TestAlreadyExist = "Test with same title already exists.";
        public static readonly string TestQuestionAlreadyExist = "Test Question already exists.";
        public static readonly string TestAlreadySubmitted = "Test Already Submitted.";
        public static readonly string PDFFileAlreadyExist = "A file with the same title already exists.";
    }
}
