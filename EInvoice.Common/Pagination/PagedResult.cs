namespace EInvoice.Common.Pagination
{
    public class PagedResult<T> : PagedResultBase where T : class
    {
        public PagedResult()
        {

        }
        public List<T> Results { get; set; }
    }
}
