using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Almacen.Models.Data
{
    public class Product
    {
        public ICollection<Contains> Contains { get; set; }
        public string Id { get; set; }
        [Required(ErrorMessage = "Por favor, ingrese una cantidad mayor o igual a 0")]
        [Range(0, 999999)]
        public int Quantity { get; set; }
        [MaxLength(50)]
        public string Category { get; set; }
        [MaxLength(250)]
        public string Description { get; set; }
        [MaxLength(50)]
        [Required(ErrorMessage = "Por favor, ingrese un nombre")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Por favor, ingrese el precio final")]
        [Range(0, 9999999999)] //Hiperinflacion
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Por favor, ingrese el precio de costo")]
        [Range(0, 9999999999)]
        public decimal CostPrice { get; set; }
        [MaxLength(50)]
        public string Brand { get; set; }
    }
}
