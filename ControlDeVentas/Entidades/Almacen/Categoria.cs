using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Almacen
{
    public class Categoria
    {
        public int IdCategoria { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener menos de tres caracteres, ni mas de 100.")]
        public string NombreCategoria { get; set; } = string.Empty;
        [StringLength(250)]
        public string Descripcion { get; set; } = string.Empty;
        public bool Estado { get; set; } = false;

        [ForeignKey("IdCategoria")]
        public ICollection<Articulo> Articulos { get; set; }
    }
}
