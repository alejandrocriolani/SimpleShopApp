using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Almacen.Models.Data
{
    public class Order
    {
        [BindNever]
        public string Id { get; set; }
        [BindNever]
        public ICollection<CartLine> Lines { get; set; }

        public User Client { get; set; }
        public User Employee { get; set; }
        public SupplierEnterprise SupplierEnterprise { get; set; }
        public bool Rejected { get; set; }
        public bool Delivered { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime OrderDateTime { get; set; }
        
    }
       
}
