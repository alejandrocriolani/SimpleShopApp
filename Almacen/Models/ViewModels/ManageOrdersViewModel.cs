using Almacen.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Almacen.Models.ViewModels
{
    public class BasicOrderData
    {
        public User Client { get; set; }
        public User Employee { get; set; }
        public Order Order { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
