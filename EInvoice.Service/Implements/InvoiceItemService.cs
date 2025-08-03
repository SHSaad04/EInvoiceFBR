using AutoMapper;
using EInvoice.Common.DTO.Filter;
using EInvoice.Common.Entities;
using EInvoice.Common.Exceptions.Types;
using EInvoice.Common.Exceptions;
using EInvoice.Common.Pagination;
using EInvoice.Domain.Entities;
using EInvoice.Infrastructure;
using EInvoice.Service.Aggregates;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EInvoice.Service.Implements
{
    public class InvoiceItemService(EInvoiceContext einvoiceContext, IMapper mapper, IConfiguration configuration) : IInvoiceItemService
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly EInvoiceContext ctx = einvoiceContext;
        private readonly IMapper mapper = mapper;
        public async Task<InvoiceItemDTO> Add(InvoiceItemDTO invoiceItemDTO)
        {
            if (AlreadyExistByTitle(invoiceItemDTO.HsCode, invoiceItemDTO.Id))
            {
                throw new UserTypeException(ExceptionMessages.RecordAlreadyExist);
            }
            var invoiceItem = mapper.Map<InvoiceItem>(invoiceItemDTO);
            ctx.Entry(invoiceItem).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            await ctx.InvoiceItems.AddAsync(invoiceItem);
            await ctx.SaveChangesAsync();
            return mapper.Map<InvoiceItemDTO>(invoiceItem);
        }
        public async Task Delete(long id)
        {
            var invoiceItem = await ctx.InvoiceItems.FindAsync(id);
            if (invoiceItem != null)
            {
                ctx.InvoiceItems.Remove(invoiceItem);
                await ctx.SaveChangesAsync();
            }
        }
        public void Dispose()
        {
            ctx?.Dispose();
        }
        public async Task<InvoiceItemDTO> Edit(InvoiceItemDTO invoiceItemDTO)
        {
            var invoiceItem = mapper.Map<InvoiceItem>(invoiceItemDTO);
            var invoiceItemOld = ctx.InvoiceItems.AsNoTracking().FirstOrDefault(x => x.Id == invoiceItem.Id);
            if (invoiceItemOld == null)
            {
                throw new UserTypeException(ExceptionMessages.DataNotFoundExceptionMessage);
            }
            if (AlreadyExistByTitle(invoiceItemDTO.HsCode, invoiceItemDTO.Id))
            {
                throw new UserTypeException(ExceptionMessages.RecordAlreadyExist);
            }
            ctx.InvoiceItems.Update(invoiceItem);
            ctx.Entry(invoiceItem).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await ctx.SaveChangesAsync();
            return mapper.Map<InvoiceItemDTO>(invoiceItem);
        }
        private bool AlreadyExistByTitle(string hsCode, long id)
        {
            return ctx.InvoiceItems.Any(x => x.HsCode == hsCode && x.Id != id);
        }
        public async Task<List<InvoiceItemDTO>> GetAll()
        {
            var invoices = await ctx.InvoiceItems.OrderBy(x => x.HsCode).ToListAsync();
            return mapper.Map<List<InvoiceItemDTO>>(invoices);
        }
        public async Task<InvoiceItemDTO> GetById(long id)
        {
            var invoiceItem = await ctx.InvoiceItems.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return mapper.Map<InvoiceItemDTO>(invoiceItem);
        }
        public async Task<PagedResult<InvoiceItemDTO>> GetByPage(int pageNumber, int pageSize)
        {
            var pagedInvoices = await ctx.InvoiceItems.PaginateAsync(pageNumber, pageSize);
            return mapper.Map<PagedResult<InvoiceItemDTO>>(pagedInvoices);
        }
        public async Task<PagedResult<InvoiceItemDTO>> GetByFilter(InvoiceItemFilterDTO filterDTO)
        {
            var invoiceQuery = ctx.InvoiceItems.Where(x => x.HsCode.Contains(filterDTO.SearchQuery)).AsNoTracking();
            if (!string.IsNullOrEmpty(filterDTO.HsCode) && filterDTO.HsCode != "All")
            {
                invoiceQuery = invoiceQuery.Where(x => x.HsCode == filterDTO.HsCode);
            }
            var invoices = await invoiceQuery.OrderBy(x => x.HsCode).PaginateAsync(filterDTO.PageNumber, filterDTO.PageSize);
            var invoiceItemDTOs = mapper.Map<PagedResult<InvoiceItemDTO>>(invoices);
            return invoiceItemDTOs;
        }
    }
}
