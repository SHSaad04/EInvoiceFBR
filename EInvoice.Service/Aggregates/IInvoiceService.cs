using EInvoice.Common.DTO.Filter;
using EInvoice.Common.Entities;
using EInvoice.Common.Pagination;
using EInvoice.Service.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EInvoice.Service.Aggregates
{
    public interface IInvoiceService : IService<InvoiceDTO>
    {
        Task<PagedResult<InvoiceDTO>> GetByFilter(InvoiceFilterDTO filterDTO);
        Task<List<InvoiceTypeDTO>> GetAllInvocieTypes();
    }
}
