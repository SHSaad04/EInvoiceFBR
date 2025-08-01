using AutoMapper;
using EInvoice.Common.DTO.Filter;
using EInvoice.Common.DTO;
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
using EInvoice.Common.Exceptions;

namespace EInvoice.Service.Implements
{
    public class OrganizationService(EInvoiceContext einvoiceContext, IMapper mapper, IConfiguration configuration) : IOrganizationService
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly EInvoiceContext ctx = einvoiceContext;
        private readonly IMapper mapper = mapper;

        public async Task<OrganizationDTO> Add(OrganizationDTO organizationDTO)
        {
            if (AlreadyExistByTitle(organizationDTO.BusinessName, organizationDTO.Id))
            {
                throw new UserTypeException(ExceptionMessages.BookAlreadyExist);
            }
            var organization = mapper.Map<Organization>(organizationDTO);
            ctx.Entry(organization).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            await ctx.Organizations.AddAsync(organization);
            await ctx.SaveChangesAsync();
            return mapper.Map<OrganizationDTO>(organization);
        }

        public async Task Delete(long id)
        {
            var organization = await ctx.Organizations.FindAsync(id);
            if (organization != null)
            {
                ctx.Organizations.Remove(organization);
                await ctx.SaveChangesAsync();
            }
        }

        public void Dispose()
        {
            ctx?.Dispose();
        }

        public async Task<OrganizationDTO> Edit(OrganizationDTO organizationDTO)
        {
            var organization = mapper.Map<Organization>(organizationDTO);
            var bookOld = ctx.Organizations.AsNoTracking().FirstOrDefault(x => x.Id == organization.Id);
            if (bookOld == null)
            {
                throw new UserTypeException(ExceptionMessages.DataNotFoundExceptionMessage);
            }
            if (AlreadyExistByTitle(organizationDTO.BusinessName, organizationDTO.Id))
            {
                throw new UserTypeException(ExceptionMessages.BookAlreadyExist);
            }
            ctx.Organizations.Update(organization);
            ctx.Entry(organization).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await ctx.SaveChangesAsync();
            return mapper.Map<OrganizationDTO>(organization);
        }

        private bool AlreadyExistByTitle(string businessName, long id)
        {
            return ctx.Organizations.Any(x => x.BusinessName == businessName && x.Id != id);
        }

        public async Task<List<OrganizationDTO>> GetAll()
        {
            var organizations = await ctx.Organizations.OrderBy(x => x.BusinessName).ToListAsync();
            return mapper.Map<List<OrganizationDTO>>(organizations);
        }

        public async Task<OrganizationDTO> GetById(long id)
        {
            var organization = await ctx.Organizations.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return mapper.Map<OrganizationDTO>(organization);
        }

        public async Task<PagedResult<OrganizationDTO>> GetByPage(int pageNumber, int pageSize)
        {
            var pagedBooks = await ctx.Organizations.PaginateAsync(pageNumber, pageSize);
            return mapper.Map<PagedResult<OrganizationDTO>>(pagedBooks);
        }

        public async Task<PagedResult<OrganizationDTO>> GetByFilter(OrganizationFilterDTO filterDTO)
        {
            var booksQuery = ctx.Organizations.Where(x => x.BusinessName.Contains(filterDTO.SearchQuery)).AsNoTracking();
            if (!string.IsNullOrEmpty(filterDTO.BusinessName) && filterDTO.BusinessName != "All")
            {
                booksQuery = booksQuery.Where(x => x.BusinessName == filterDTO.BusinessName);
            }
            var organizations = await booksQuery.OrderBy(x => x.BusinessName).PaginateAsync(filterDTO.PageNumber, filterDTO.PageSize);
            var bookDTOs = mapper.Map<PagedResult<OrganizationDTO>>(organizations);
            return bookDTOs;
        }
    }
}
