using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Almacen.Models.Data;

namespace Almacen.Models
{
    public class ProductRepository : IProductRepository
    {
        private AlmacenDbContext context;

        public ProductRepository(AlmacenDbContext ctx)
        {
            context = ctx;
        }

        public IQueryable<Product> Products => context.Products;

        public void SaveProduct(Product product)
        {
            if(string.IsNullOrEmpty(product.Id))
            {
                context.Products.Add(product);
            }
            else
            {
                Product dbEntry = context.Products.FirstOrDefault(
                    p => p.Id == product.Id);
                if(dbEntry != null)
                {
                    dbEntry.Name = product.Name;
                    dbEntry.Description = product.Description;
                    dbEntry.Brand = product.Brand;
                    dbEntry.Category = product.Category;
                    dbEntry.CostPrice = product.CostPrice;
                    dbEntry.Price = product.Price;
                    dbEntry.Quantity = product.Quantity;
                }
            }
            context.SaveChanges();
        }
    }
}
