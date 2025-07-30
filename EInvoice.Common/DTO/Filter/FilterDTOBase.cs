using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EInvoice.Common.DTO.Filter
{
    public class FilterDTOBase
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchQuery { get; set; } = string.Empty;
        public string SortActive { get; set; } = string.Empty;
        public string SortDirection { get; set; } = "asc";
    }
}
