using AutoMapper;
using EInvoice.Common.DTO;
using EInvoice.Common.DTO.Filter;
using EInvoice.Common.Entities;
using EInvoice.Common.Exceptions.Types;
using EInvoice.Common.Pagination;
using EInvoice.Domain.Entities;
using EInvoice.Infrastructure;
using EInvoice.Service.Aggregates;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EInvoice.Service.Implements
{
    public class UserService(EInvoiceContext einvoiceContext, IMapper mapper, UserManager<User> userManager, IConfiguration configuration) : IUserService
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly IConfiguration _configuration = configuration;
        private readonly EInvoiceContext ctx = einvoiceContext;
        private readonly IMapper mapper = mapper;

        public async Task<UserDTO> Add(UserDTO userDTO)
        {
            var user = mapper.Map<User>(userDTO);
            user.EmailConfirmed = true;            // Skips email confirmation.
            user.PhoneNumberConfirmed = true;      // Skips phone number confirmation.
            user.TwoFactorEnabled = false;         // No two-factor authentication required.
            user.LockoutEnabled = false;           // User cannot be locked out.
            user.AccessFailedCount = 0;
            user.AvatarURL = "image.jpg";
            var result = await _userManager.CreateAsync(user);

            if (!result.Succeeded)
                throw new UserTypeException(string.Join("\n", result.Errors.Select(e => e.Description)));

            await _userManager.AddToRoleAsync(user, "Admin");
            return mapper.Map<UserDTO>(user);
        }

        public async Task<AuthenticateResponseDTO> Authenticate(AuthenticateRequestDTO request)
        {
            var user = await _userManager.FindByNameAsync(request.Username);

            if (user == null)
                return null;

            var result = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!result)
                return null;

            //user.LastLogin = DateTime.UtcNow;
            //user.IpAddress = request.IpAddress;
            //user.Device = request.Device;
            //if (user.IsFirstLogin)
            //{

            //}
            //await _userManager.UpdateAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);
            var role = "Client";
            if (userRoles != null && userRoles.Any())
            {
                role = userRoles.FirstOrDefault();
            }
            var token = GenerateJwtToken(user, role);

            return new AuthenticateResponseDTO
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = token
            };
        }

        public async Task Delete(long id)
        {
            var user = await ctx.Users.FindAsync(id);
            if (user != null)
            {
                ctx.Users.Remove(user);
            }
        }

        public void Dispose()
        {
            ctx?.Dispose();
        }

        public async Task<UserDTO> Edit(UserDTO userDTO)
        {
            var user = await _userManager.FindByIdAsync(userDTO.Id);

            if (user == null)
                throw new UserTypeException("User not found");

            mapper.Map(userDTO, user);  // Updates fields from dto to user
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                throw new UserTypeException(string.Join("\n", result.Errors.Select(e => e.Description)));

            return mapper.Map<UserDTO>(user);
        }

        public async Task<List<UserDTO>> GetAll()
        {
            var pages = await ctx.Users.ToListAsync();
            return mapper.Map<List<UserDTO>>(pages);
        }

        public async Task<UserDTO> GetById(long id)
        {
            var page = await ctx.Users.FindAsync(id);
            return mapper.Map<UserDTO>(page);
        }

        public async Task<PagedResult<UserDTO>> GetByPage(int pageNumber, int pageSize)
        {
            var pagedUsers = await ctx.Users.PaginateAsync(pageNumber, pageSize);
            return mapper.Map<PagedResult<UserDTO>>(pagedUsers);
        }

        public async Task<int> Signup(UserDTO userDTO)
        {
            var user = new User
            {
                UserName = userDTO.UserName,
                Email = userDTO.Email,
                FirstName = userDTO.FirstName,
                LastName = userDTO.LastName,
                Contact = userDTO.Contact,
                Country = userDTO.Country,
                City = userDTO.City,
                AvatarURL = "/Content/img/default.webp",
                IsAdmin = null,
                PromoCode = userDTO.PromoCode
            };

            var result = await _userManager.CreateAsync(user, userDTO.Password);

            if (!result.Succeeded)
            {
                throw new UserTypeException(string.Join("\n", result.Errors.Select(e => e.Description)));
            }
            await _userManager.AddToRoleAsync(user, "Client");
            return user.Id.GetHashCode();
        }

        private string GenerateJwtToken(User user, string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Role, role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public async Task<PagedResult<UserDTO>> GetByFilter(UserFilterDTO filterDTO)
        {
            var usersQuery = ctx.Users.Where(x => x.Email.Contains(filterDTO.SearchQuery)).AsNoTracking();
            if (!string.IsNullOrEmpty(filterDTO.SortActive))
            {
                //pagesQuery = pagesQuery.OrderByString(filterDTO.SortActive + " " + filterDTO.SortDirection);
            }
            var users = await usersQuery.PaginateAsync(filterDTO.PageNumber, filterDTO.PageSize);
            var userDTOs = mapper.Map<PagedResult<UserDTO>>(users);
            return userDTOs;
        }
    }
}
