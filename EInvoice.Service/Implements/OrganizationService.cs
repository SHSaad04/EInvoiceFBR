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
using Microsoft.AspNetCore.Http;
using EInvoice.Service.Helpers;

namespace EInvoice.Service.Implements
{
    public class OrganizationService(EInvoiceContext einvoiceContext, IMapper mapper, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : IOrganizationService
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly EInvoiceContext ctx = einvoiceContext;
        private readonly IMapper mapper = mapper;
        private long? OrganizationId = httpContextAccessor.HttpContext?.User.GetOrganizationId();
        public async Task<OrganizationDTO> Add(OrganizationDTO organizationDTO)
        {
            if (AlreadyExistByTitle(organizationDTO.NTNCNIC, organizationDTO.Id))
            {
                throw new UserTypeException(ExceptionMessages.OrganizationAlreadyExist);
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
            var organizationOld = ctx.Organizations.AsNoTracking().FirstOrDefault(x => x.Id == organization.Id);
            if (organizationOld == null)
            {
                throw new UserTypeException(ExceptionMessages.DataNotFoundExceptionMessage);
            }
            if (AlreadyExistByTitle(organizationDTO.NTNCNIC, organizationDTO.Id))
            {
                throw new UserTypeException(ExceptionMessages.OrganizationAlreadyExist);
            }
            ctx.Organizations.Update(organization);
            ctx.Entry(organization).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await ctx.SaveChangesAsync();
            return mapper.Map<OrganizationDTO>(organization);
        }
        private bool AlreadyExistByTitle(string ntncnic, long id)
        {
            return ctx.Organizations.Any(x => x.NTNCNIC == ntncnic && x.Id != id);
        }
        public async Task<List<OrganizationDTO>> GetAll()
        {
            var organizations = await ctx.Organizations.OrderBy(x => x.BusinessName).ToListAsync();
            return mapper.Map<List<OrganizationDTO>>(organizations);
        }
        public async Task<OrganizationDTO> GetById(long id)
        {

            var organization = await ctx.Organizations.AsNoTracking().FirstOrDefaultAsync(x => x.Id == OrganizationId);
            return mapper.Map<OrganizationDTO>(organization);
        }
        public async Task<PagedResult<OrganizationDTO>> GetByPage(int pageNumber, int pageSize)
        {
            var pagedOrganizations = await ctx.Organizations.PaginateAsync(pageNumber, pageSize);
            return mapper.Map<PagedResult<OrganizationDTO>>(pagedOrganizations);
        }
        public async Task<PagedResult<OrganizationDTO>> GetByFilter(OrganizationFilterDTO filterDTO)
        {
            var organizationQuery = ctx.Organizations.Where(x => x.BusinessName.Contains(filterDTO.SearchQuery)).AsNoTracking();
            if (!string.IsNullOrEmpty(filterDTO.BusinessName) && filterDTO.BusinessName != "All")
            {
                organizationQuery = organizationQuery.Where(x => x.BusinessName == filterDTO.BusinessName);
            }
            var organizations = await organizationQuery.OrderBy(x => x.BusinessName).PaginateAsync(filterDTO.PageNumber, filterDTO.PageSize);
            var organizationDTOs = mapper.Map<PagedResult<OrganizationDTO>>(organizations);
            return organizationDTOs;
        }
    }
}
