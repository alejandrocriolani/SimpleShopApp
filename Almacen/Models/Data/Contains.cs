using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Almacen.Models.Data
{
    public class Contains
    {
        public string Id { get; set; }
        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}
