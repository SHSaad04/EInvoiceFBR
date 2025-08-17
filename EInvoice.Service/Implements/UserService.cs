using AutoMapper;
using EInvoice.Common.DTO;
using EInvoice.Common.DTO.Filter;
using EInvoice.Common.DTO;
using EInvoice.Common.Exceptions.Types;
using EInvoice.Common.Pagination;
using EInvoice.Domain.Entities;
using EInvoice.Infrastructure;
using EInvoice.Service.Aggregates;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EInvoice.Service.Implements
{
    public class UserService(EInvoiceContext einvoiceContext, IMapper mapper, UserManager<User> userManager, IConfiguration configuration, SignInManager<User> signInManager) : IUserService
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly SignInManager<User> _signInManager = signInManager;
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

            await _userManager.AddToRoleAsync(user, UserRoles.OrganizationAdmin);
            return mapper.Map<UserDTO>(user);
        }

        public async Task<AuthenticateResponseDTO> Authenticate(AuthenticateRequestDTO request)
        {
            var user = await _userManager.FindByNameAsync(request.Username)
                        ?? await _userManager.FindByEmailAsync(request.Username);

            if (user == null)
                return new AuthenticateResponseDTO
                {
                    Success = false,
                    Message = "User not found"
                };

            // Sign in using cookie authentication
            var result = await _signInManager.PasswordSignInAsync(
                user,
                request.Password,
                isPersistent: false,   // Whether to persist the cookie after browser is closed
                lockoutOnFailure: false);

            if (!result.Succeeded)
                return new AuthenticateResponseDTO
                {
                    Success = false,
                    Message = "Invalid credentials"
                };

            // No manual claim manipulation here at all.
            // The AppClaimsPrincipalFactory will handle adding IsOrganizationAssociated + OrganizationId claims.

            // Refresh sign-in to make sure the cookie is rebuilt with claims from AppClaimsPrincipalFactory
            await _signInManager.RefreshSignInAsync(user);

            return new AuthenticateResponseDTO
            {
                Success = true,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsOrganizationAssociated = user.OrganizationId != null
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

        public async Task<UserDTO> GetById(string id)
        {
            var page = await ctx.Users.FindAsync(id);
            return mapper.Map<UserDTO>(page);
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

        public async Task<string> Signup(UserDTO userDTO)
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
            try
            {

                await _userManager.AddToRoleAsync(user, UserRoles.OrganizationAdmin);
            }
            catch (Exception ex)
            {
                throw new UserTypeException(string.Join("\n", ex.ToString()));
            }
            return user.Id;
        }
        public async Task<bool> Signout(string userId)
        {
            //var user = await _userManager.FindByIdAsync(userId);
            //if (user == null)
            //    return false;
            await signInManager.SignOutAsync();
            return true;
        }

        private string GenerateJwtToken(User user, string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Role, role)
            };
            // Add OrganizationId if present
            if (user.OrganizationId.HasValue)
                claims.Add(new Claim("OrganizationId", user.OrganizationId.Value.ToString()));
            else
                claims.Add(new Claim("OrgPending", "true"));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
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
        public async Task<bool> UpdateClaims(UserDTO userDTO)
        {
            var user = await _userManager.FindByIdAsync(userDTO.Id.ToString());
            if (user == null) return false;

            var existingClaims = await _userManager.GetClaimsAsync(user);

            var oldClaims = existingClaims
                .Where(c => c.Type == "IsOrganizationAssociated" || c.Type == "OrganizationId")
                .ToList();

            foreach (var claim in oldClaims)
            {
                await _userManager.RemoveClaimAsync(user, claim);
            }

            var claims = new List<Claim>
            {
                new Claim("IsOrganizationAssociated", "true"),
                new Claim("OrganizationId", userDTO.OrganizationId.ToString())
            };

            foreach (var claim in claims)
            {
                await _userManager.AddClaimAsync(user, claim);
            }
            await _signInManager.RefreshSignInAsync(user);

            return true;
        }
        public async Task<UserDTO?> FindByEmailAsync(string email)
        {
            User user = new User();
            user = await _userManager.FindByEmailAsync(email);
            UserDTO userDTO = new UserDTO();
            mapper.Map(user, userDTO);
            return userDTO;
        }

        public async Task<string> GeneratePasswordResetTokenAsync(UserDTO userDTO)
        {
            User user = new User();
            mapper.Map(userDTO, user);
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<IdentityResult> ResetPasswordAsync(UserDTO userDTO, string token, string newPassword)
        {
            User user = new User();
            mapper.Map(userDTO, user);
            return await _userManager.ResetPasswordAsync(user, token, newPassword);
        }

    }
}
