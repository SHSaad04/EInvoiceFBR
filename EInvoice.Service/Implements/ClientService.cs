using AutoMapper;
using EInvoice.Common.DTO.Filter;
using EInvoice.Common.Entities;
using EInvoice.Common.Exceptions.Types;
using EInvoice.Common.Exceptions;
using EInvoice.Common.Pagination;
using EInvoice.Domain.Entities;
using EInvoice.Infrastructure;
using EInvoice.Service.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using EInvoice.Service.Helpers;

namespace EInvoice.Service.Implements
{
    public class ClientService(EInvoiceContext einvoiceContext, IMapper mapper, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : IClientService
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly EInvoiceContext ctx = einvoiceContext;
        private readonly IMapper mapper = mapper;
        private long? OrganizationId = httpContextAccessor.HttpContext?.User.GetOrganizationId();

        public async Task<ClientDTO> Add(ClientDTO clientDTO)
        {
            if (AlreadyExistByTitle(clientDTO.NTNCNIC, clientDTO.Id))
            {
                throw new UserTypeException(ExceptionMessages.ClientAlreadyExist);
            }
            var client = mapper.Map<Client>(clientDTO);
            client.OrganizationId = OrganizationId;
            ctx.Entry(client).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            await ctx.Clients.AddAsync(client);
            await ctx.SaveChangesAsync();
            return mapper.Map<ClientDTO>(client);
        }
        public async Task Delete(long id)
        {
            var client = await ctx.Clients.FindAsync(id);
            if (client != null)
            {
                ctx.Clients.Remove(client);
                await ctx.SaveChangesAsync();
            }
        }
        public void Dispose()
        {
            ctx?.Dispose();
        }
        public async Task<ClientDTO> Edit(ClientDTO clientDTO)
        {
            var client = mapper.Map<Client>(clientDTO);
            var clientOld = ctx.Clients.AsNoTracking().FirstOrDefault(x => x.Id == client.Id && x.OrganizationId == OrganizationId);
            if (clientOld == null)
            {
                throw new UserTypeException(ExceptionMessages.DataNotFoundExceptionMessage);
            }
            if (AlreadyExistByTitle(clientDTO.NTNCNIC, clientDTO.Id))
            {
                throw new UserTypeException(ExceptionMessages.ClientAlreadyExist);
            }
            ctx.Clients.Update(client);
            ctx.Entry(client).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await ctx.SaveChangesAsync();
            return mapper.Map<ClientDTO>(client);
        }
        private bool AlreadyExistByTitle(string ntncnic, long id)
        {
            return ctx.Clients.Any(x => x.NTNCNIC == ntncnic && x.Id != id);
        }
        public async Task<List<ClientDTO>> GetAll()
        {
            var clients = await ctx.Clients.Where(x=>x.OrganizationId == OrganizationId).OrderBy(x => x.BusinessName).ToListAsync();
            return mapper.Map<List<ClientDTO>>(clients);
        }
        public async Task<ClientDTO> GetById(long id)
        {
            var client = await ctx.Clients.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id && x.OrganizationId == OrganizationId);
            return mapper.Map<ClientDTO>(client);
        }
        public async Task<PagedResult<ClientDTO>> GetByPage(int pageNumber, int pageSize)
        {
            var pagedClients = await ctx.Clients.Where(x => x.OrganizationId == OrganizationId).PaginateAsync(pageNumber, pageSize);
            return mapper.Map<PagedResult<ClientDTO>>(pagedClients);
        }
        public async Task<PagedResult<ClientDTO>> GetByFilter(ClientFilterDTO filterDTO)
        {
            var clientsQuery = ctx.Clients.Where(x =>x.OrganizationId == OrganizationId && x.BusinessName.Contains(filterDTO.SearchQuery)).AsNoTracking();
            if (!string.IsNullOrEmpty(filterDTO.BusinessName) && filterDTO.BusinessName != "All")
            {
                clientsQuery = clientsQuery.Where(x => x.BusinessName == filterDTO.BusinessName);
            }
            var clients = await clientsQuery.OrderBy(x => x.BusinessName).PaginateAsync(filterDTO.PageNumber, filterDTO.PageSize);
            var clientDTOs = mapper.Map<PagedResult<ClientDTO>>(clients);
            return clientDTOs;
        }
    }
}
