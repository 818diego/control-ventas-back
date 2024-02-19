using Entidades.Almacen;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Articulo
    {
        [Key]
        public int IdArticulo { get; set; }
        [ForeignKey("IdCategoria")]
        [Required]
        public int IdCategoria { get; set; }
        [Required]
        public string CodigoArticulo { get; set; } = string.Empty;
        [Required]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "El nombre debe tener menos de tres caracteres, ni mas de 150.")]
        public string NombreArticulo { get; set; } = string.Empty;
        [Required]
        public decimal PrecioVenta { get; set; }
        [Required]
        public int Stock { get; set; }
       
        public string DescripcionArticulo { get; set; } = string.Empty;
        public bool Estado { get; set; }=false;

        public Categoria Categoria { get; set; }

        [ForeignKey("IdArticulo")]
        public ICollection<DetalleVenta> DetalleVentas { get; set; }

        [ForeignKey("IdArticulo")]
        public ICollection<DetalleIngreso> DetalleIngreso { get; set; }
    }
}
