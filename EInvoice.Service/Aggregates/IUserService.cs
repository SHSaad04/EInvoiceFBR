using EInvoice.Common.DTO.Filter;
using EInvoice.Common.DTO;
using EInvoice.Common.DTO;
using EInvoice.Common.Pagination;
using EInvoice.Service.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EInvoice.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace EInvoice.Service.Aggregates
{
    public interface IUserService : IService<UserDTO>
    {
        Task<string> Signup(UserDTO request);
        Task<AuthenticateResponseDTO> Authenticate(AuthenticateRequestDTO request);
        Task<PagedResult<UserDTO>> GetByFilter(UserFilterDTO filterDTO);
        Task<bool> Signout(string userId);
        Task<bool> UpdateClaims(UserDTO user);
        Task<UserDTO> GetById(string id);
        Task<UserDTO?> FindByEmailAsync(string email);
        Task<string> GeneratePasswordResetTokenAsync(UserDTO user);
        Task<IdentityResult> ResetPasswordAsync(UserDTO user, string token, string password);
    }
}
