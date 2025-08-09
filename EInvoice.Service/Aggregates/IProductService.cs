using EInvoice.Common.DTO.Filter;
using EInvoice.Common.DTO;
using EInvoice.Common.Pagination;
using EInvoice.Service.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EInvoice.Service.Aggregates
{
    public interface IProductService : IService<ProductDTO>
    {
        Task<PagedResult<ProductDTO>> GetByFilter(ProductFilterDTO filterDTO);
    }
}
