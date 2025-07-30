using AutoMapper;
using EInvoice.Common.Entities;
using EInvoice.Common.Pagination;
using EInvoice.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EInvoice.Service.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ClientDTO, Client>().ReverseMap();
            CreateMap<PagedResult<ClientDTO>, PagedResult<Client>>().ReverseMap();

            CreateMap<InvoiceDTO, Invoice>().ReverseMap();
            CreateMap<PagedResult<InvoiceDTO>, PagedResult<Invoice>>().ReverseMap();

            CreateMap<InvoiceItemDTO, InvoiceItem>().ReverseMap();
            CreateMap<PagedResult<InvoiceItemDTO>, PagedResult<InvoiceItem>>().ReverseMap();

            CreateMap<OrganizationDTO, Organization>().ReverseMap();
            CreateMap<PagedResult<OrganizationDTO>, PagedResult<Organization>>().ReverseMap();

            CreateMap<OrganizationDTO, Organization>().ReverseMap();
            CreateMap<PagedResult<OrganizationDTO>, PagedResult<Organization>>().ReverseMap();

            CreateMap<UserDTO, User>().ReverseMap();
            CreateMap<PagedResult<UserDTO>, PagedResult<User>>().ReverseMap();
        }
    }
}
