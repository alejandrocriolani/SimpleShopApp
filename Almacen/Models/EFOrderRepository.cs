using Almacen.Models.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Almacen.Models
{
    public class EFOrderRepository : IOrderRepository
    {
        private AlmacenDbContext context;

        public EFOrderRepository(AlmacenDbContext ctx)
        {
            context = ctx;
        }

        public IQueryable<Order> Orders => context.Orders
            .Include(o => o.Lines)
            .ThenInclude(l => l.Product);

        public void SaveOrder(Order order)
        {
            context.AttachRange(order.Lines.Select(l => l.Product));
            if(string.IsNullOrEmpty(order.Id))
            {
                context.Orders.Add(order);
            }
            context.SaveChanges();
        }
    }
}
