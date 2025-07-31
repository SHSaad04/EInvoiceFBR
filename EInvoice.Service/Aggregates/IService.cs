using EInvoice.Common.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EInvoice.Service.Aggregates
{
    public interface IService<T> : IDisposable where T : class
    {
        Task<T> GetById(long id);
        Task<List<T>> GetAll();
        Task<PagedResult<T>> GetByPage(int pageNumber, int pageSize);
        Task<T> Add(T DTO);
        Task<T> Edit(T DTO);
        Task Delete(long id);
    }
}
