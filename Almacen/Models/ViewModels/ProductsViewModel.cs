using Almacen.Models.Data;
using System.Linq;

namespace Almacen.Models.ViewModels
{
    public class ProductsViewModel
    {
        public IQueryable<Product> Products { get; set; }
        public int CurrentPage { get; set; }
        public int MaxPage { get; set; }
        public string LastSortParam { get; set; }
    }
}
