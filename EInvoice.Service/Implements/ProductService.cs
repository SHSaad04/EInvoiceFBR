using AutoMapper;
using EInvoice.Common.DTO.Filter;
using EInvoice.Common.DTO;
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
    public class ProductService(EInvoiceContext einvoiceContext, IMapper mapper, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : IProductService
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly EInvoiceContext ctx = einvoiceContext;
        private readonly IMapper mapper = mapper;
        private long? OrganizationId = httpContextAccessor.HttpContext?.User.GetOrganizationId();

        public async Task<ProductDTO> Add(ProductDTO productDTO)
        {
            if (AlreadyExistByTitle(productDTO.HsCode, productDTO.Id))
            {
                throw new UserTypeException(ExceptionMessages.ProductAlreadyExist);
            }
            var product = mapper.Map<Product>(productDTO);
            product.OrganizationId = OrganizationId;
            ctx.Entry(product).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            await ctx.Products.AddAsync(product);
            await ctx.SaveChangesAsync();
            return mapper.Map<ProductDTO>(product);
        }
        public async Task Delete(long id)
        {
            var product = await ctx.Products.FindAsync(id);
            if (product != null)
            {
                ctx.Products.Remove(product);
                await ctx.SaveChangesAsync();
            }
        }
        public void Dispose()
        {
            ctx?.Dispose();
        }
        public async Task<ProductDTO> Edit(ProductDTO productDTO)
        {
            var product = mapper.Map<Product>(productDTO);
            product.OrganizationId = OrganizationId;
            var productOld = ctx.Products.AsNoTracking().FirstOrDefault(x => x.Id == product.Id && x.OrganizationId == OrganizationId);
            if (productOld == null)
            {
                throw new UserTypeException(ExceptionMessages.DataNotFoundExceptionMessage);
            }
            if (AlreadyExistByTitle(productDTO.HsCode, productDTO.Id))
            {
                throw new UserTypeException(ExceptionMessages.ProductAlreadyExist);
            }
            ctx.Products.Update(product);
            ctx.Entry(product).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await ctx.SaveChangesAsync();
            return mapper.Map<ProductDTO>(product);
        }
        private bool AlreadyExistByTitle(string HsCode, long id)
        {
            return ctx.Products.Any(x => x.HsCode == HsCode && x.Id != id);
        }
        public async Task<List<ProductDTO>> GetAll()
        {
            var products = await ctx.Products.Where(x=>x.OrganizationId == OrganizationId).OrderBy(x => x.HsCode).ToListAsync();
            return mapper.Map<List<ProductDTO>>(products);
        }
        public async Task<List<ProductDTO>> GetDropdown()
        {
            var products = await ctx.Products
                .Where(x => x.OrganizationId == OrganizationId)
                .OrderBy(x => x.HsCode)
                .Select(x => new ProductDTO
                {
                    Id = x.Id,
                    ProductDescription = $"({x.HsCode}) {x.productDescription}"
                })
                .ToListAsync();

            return products;
        }
        public async Task<ProductDTO> GetById(long id)
        {
            var product = await ctx.Products.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id && x.OrganizationId == OrganizationId);
            return mapper.Map<ProductDTO>(product);
        }
        public async Task<PagedResult<ProductDTO>> GetByPage(int pageNumber, int pageSize)
        {
            var pagedProducts = await ctx.Products.Where(x => x.OrganizationId == OrganizationId).PaginateAsync(pageNumber, pageSize);
            return mapper.Map<PagedResult<ProductDTO>>(pagedProducts);
        }
        public async Task<PagedResult<ProductDTO>> GetByFilter(ProductFilterDTO filterDTO)
        {
            var productsQuery = ctx.Products.Where(x =>x.OrganizationId == OrganizationId && x.HsCode.Contains(filterDTO.SearchQuery)).AsNoTracking();
            if (!string.IsNullOrEmpty(filterDTO.HsCode) && filterDTO.HsCode != "All")
            {
                productsQuery = productsQuery.Where(x => x.HsCode == filterDTO.HsCode);
            }
            var products = await productsQuery.OrderBy(x => x.HsCode).PaginateAsync(filterDTO.PageNumber, filterDTO.PageSize);
            var productDTOs = mapper.Map<PagedResult<ProductDTO>>(products);
            return productDTOs;
        }
    }
}
