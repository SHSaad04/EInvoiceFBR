using EInvoice.Common.Entities;
using EInvoice.Service.Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EInvoice.Service.Aggregates
{
    public interface IUserService : IService<UserDTO>
    {
    }
}
