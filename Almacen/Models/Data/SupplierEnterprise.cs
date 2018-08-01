using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Almacen.Models.Data
{
    public class SupplierEnterprise
    {
        public string Id { get; set; }
        [MaxLength(250)]
        public string Description { get; set; }
        [MaxLength(100)]
        public string Address { get; set; }
        [MaxLength(60)]
        public string Email { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(25)]
        public string Phone { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
