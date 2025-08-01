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
    public class InvoiceService(EInvoiceContext einvoiceContext, IMapper mapper, IConfiguration configuration) : IInvoiceService
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly EInvoiceContext ctx = einvoiceContext;
        private readonly IMapper mapper = mapper;
        public async Task<InvoiceDTO> Add(InvoiceDTO invoiceDTO)
        {
            if (AlreadyExistByTitle(invoiceDTO.InvoiceRefNo, invoiceDTO.Id))
            {
                throw new UserTypeException(ExceptionMessages.BookAlreadyExist);
            }
            var invoice = mapper.Map<Invoice>(invoiceDTO);
            ctx.Entry(invoice).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            await ctx.Invoices.AddAsync(invoice);
            await ctx.SaveChangesAsync();
            return mapper.Map<InvoiceDTO>(invoice);
        }
        public async Task Delete(long id)
        {
            var invoice = await ctx.Invoices.FindAsync(id);
            if (invoice != null)
            {
                ctx.Invoices.Remove(invoice);
                await ctx.SaveChangesAsync();
            }
        }
        public void Dispose()
        {
            ctx?.Dispose();
        }
        public async Task<InvoiceDTO> Edit(InvoiceDTO invoiceDTO)
        {
            var invoice = mapper.Map<Invoice>(invoiceDTO);
            var bookOld = ctx.Invoices.AsNoTracking().FirstOrDefault(x => x.Id == invoice.Id);
            if (bookOld == null)
            {
                throw new UserTypeException(ExceptionMessages.DataNotFoundExceptionMessage);
            }
            if (AlreadyExistByTitle(invoiceDTO.InvoiceRefNo, invoiceDTO.Id))
            {
                throw new UserTypeException(ExceptionMessages.BookAlreadyExist);
            }
            ctx.Invoices.Update(invoice);
            ctx.Entry(invoice).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await ctx.SaveChangesAsync();
            return mapper.Map<InvoiceDTO>(invoice);
        }
        private bool AlreadyExistByTitle(string invoiceRefNo, long id)
        {
            return ctx.Invoices.Any(x => x.InvoiceRefNo == invoiceRefNo && x.Id != id);
        }
        public async Task<List<InvoiceDTO>> GetAll()
        {
            var invoices = await ctx.Invoices.OrderBy(x => x.InvoiceRefNo).ToListAsync();
            return mapper.Map<List<InvoiceDTO>>(invoices);
        }
        public async Task<InvoiceDTO> GetById(long id)
        {
            var invoice = await ctx.Invoices.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return mapper.Map<InvoiceDTO>(invoice);
        }
        public async Task<PagedResult<InvoiceDTO>> GetByPage(int pageNumber, int pageSize)
        {
            var pagedInvoices = await ctx.Invoices.PaginateAsync(pageNumber, pageSize);
            return mapper.Map<PagedResult<InvoiceDTO>>(pagedInvoices);
        }
        public async Task<PagedResult<InvoiceDTO>> GetByFilter(InvoiceFilterDTO filterDTO)
        {
            var invoiceQuery = ctx.Invoices.Where(x => x.InvoiceRefNo.Contains(filterDTO.SearchQuery)).AsNoTracking();
            if (!string.IsNullOrEmpty(filterDTO.InvoiceRefNo) && filterDTO.InvoiceRefNo != "All")
            {
                invoiceQuery = invoiceQuery.Where(x => x.InvoiceRefNo == filterDTO.InvoiceRefNo);
            }
            var invoices = await invoiceQuery.OrderBy(x => x.InvoiceRefNo).PaginateAsync(filterDTO.PageNumber, filterDTO.PageSize);
            var invoiceDTOs = mapper.Map<PagedResult<InvoiceDTO>>(invoices);
            return invoiceDTOs;
        }
    }
}
