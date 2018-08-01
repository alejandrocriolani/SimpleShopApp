using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Almacen.Models.ViewModels
{
    public class UserEditViewModel
    {
        public string Address { get; set; }
        public string City { get; set; }
        public string DNI { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        [Required]
        public string Id { get; set; }
        public string Password { get; set; }
        public string Province { get; set; }
        public string UserName { get; set; }
        public string ZipCode { get; set; }

        public string ReturnUrl { get; set; }
    }
}
