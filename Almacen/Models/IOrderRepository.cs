using System;
using System.Collections.Generic;
using System.Linq;
using Almacen.Models.Data;
using System.Threading.Tasks;

namespace Almacen.Models
{
    public interface IOrderRepository
    {
        IQueryable<Order> Orders { get; }
        void SaveOrder(Order order);
    }
}
