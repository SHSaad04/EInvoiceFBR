using EInvoice.Common.DTO.Filter;
using EInvoice.Common.DTO;
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
    public interface IUserService : IService<UserDTO>
    {
        Task<int> Signup(UserDTO request);
        Task<AuthenticateResponseDTO> Authenticate(AuthenticateRequestDTO request);
        Task<PagedResult<UserDTO>> GetByFilter(UserFilterDTO filterDTO);
    }
}
