using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Almacen.Models.Data
{
    public class Make
    {
        public string Id { get; set; }
        public SupplierEnterprise SupplierEnterprise { get; set; }
        public User Administrator { get; set; }
        public Order Order { get; set; }
    }
}
