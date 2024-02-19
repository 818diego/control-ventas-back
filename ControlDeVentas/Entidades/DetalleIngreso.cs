using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class DetalleIngreso
    {
        [Key]
        public int IdDetalleIngreso { get; set; }
        [Required]
        [ForeignKey("IdIngreso")]
        public int IdIngreso { get; set; }
        [Required]
        [ForeignKey("IdArticulo")]
        public int IdArticulo { get; set; }
        [Required]
        public int Cantidad { get; set; }
        [Required]
        public decimal Precio { get; set; }
        
        public Ingreso Ingreso{ get; set; }
        public Articulo Articulo { get; set; }

    }
}
