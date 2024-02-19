using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class DetalleVenta
    {
        [Key]
        public int IdDetalleVentas { get; set; }
        [Required]
        [ForeignKey("IdVenta")]
        public int IdVenta { get; set; }
        [Required]
        [ForeignKey("IdArticulo")]
        public int IdArticulo { get; set; }
        [Required]
        public int Cantidad { get; set; }
        [Required]
        public decimal Descuento { get; set; }
        public Venta Venta { get; set; }
        public Articulo Articulo { get; set; }

    }
}
