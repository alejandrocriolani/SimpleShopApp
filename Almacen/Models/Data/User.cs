using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;

namespace Almacen.Models.Data
{
    public class User : IdentityUser
    {
        [Required]
        public string Address { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Province { get; set; }
        [Required]
        [MaxLength(5)]
        public string ZipCode { get; set; }
        [MaxLength(12)]
        public string DNI { get; set; }
        [MaxLength(75)]
        public string Name { get; set; }
        [MaxLength(50)]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime RegisterDate { get; set; }

        public ICollection<Order> Orders { get; set; }
        public SupplierEnterprise SupplierEnterprise { get; set; }
        public ICollection<Make> Makes { get; set; }
    }
}
