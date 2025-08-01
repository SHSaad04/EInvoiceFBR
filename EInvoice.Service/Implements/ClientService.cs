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

namespace EInvoice.Service.Implements
{
    public class ClientService(EInvoiceContext einvoiceContext, IMapper mapper, IConfiguration configuration) : IClientService
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly EInvoiceContext ctx = einvoiceContext;
        private readonly IMapper mapper = mapper;
        public async Task<ClientDTO> Add(ClientDTO clientDTO)
        {
            if (AlreadyExistByTitle(clientDTO.BusinessName, clientDTO.Id))
            {
                throw new UserTypeException(ExceptionMessages.BookAlreadyExist);
            }
            var client = mapper.Map<Client>(clientDTO);
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
            var bookOld = ctx.Clients.AsNoTracking().FirstOrDefault(x => x.Id == client.Id);
            if (bookOld == null)
            {
                throw new UserTypeException(ExceptionMessages.DataNotFoundExceptionMessage);
            }
            if (AlreadyExistByTitle(clientDTO.BusinessName, clientDTO.Id))
            {
                throw new UserTypeException(ExceptionMessages.BookAlreadyExist);
            }
            ctx.Clients.Update(client);
            ctx.Entry(client).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await ctx.SaveChangesAsync();
            return mapper.Map<ClientDTO>(client);
        }
        private bool AlreadyExistByTitle(string businessName, long id)
        {
            return ctx.Clients.Any(x => x.BusinessName == businessName && x.Id != id);
        }
        public async Task<List<ClientDTO>> GetAll()
        {
            var organizations = await ctx.Clients.OrderBy(x => x.BusinessName).ToListAsync();
            return mapper.Map<List<ClientDTO>>(organizations);
        }
        public async Task<ClientDTO> GetById(long id)
        {
            var client = await ctx.Clients.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return mapper.Map<ClientDTO>(client);
        }
        public async Task<PagedResult<ClientDTO>> GetByPage(int pageNumber, int pageSize)
        {
            var pagedBooks = await ctx.Clients.PaginateAsync(pageNumber, pageSize);
            return mapper.Map<PagedResult<ClientDTO>>(pagedBooks);
        }
        public async Task<PagedResult<ClientDTO>> GetByFilter(OrganizationFilterDTO filterDTO)
        {
            var booksQuery = ctx.Clients.Where(x => x.BusinessName.Contains(filterDTO.SearchQuery)).AsNoTracking();
            if (!string.IsNullOrEmpty(filterDTO.BusinessName) && filterDTO.BusinessName != "All")
            {
                booksQuery = booksQuery.Where(x => x.BusinessName == filterDTO.BusinessName);
            }
            var organizations = await booksQuery.OrderBy(x => x.BusinessName).PaginateAsync(filterDTO.PageNumber, filterDTO.PageSize);
            var bookDTOs = mapper.Map<PagedResult<ClientDTO>>(organizations);
            return bookDTOs;
        }
    }
}
