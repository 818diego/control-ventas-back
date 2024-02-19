using Entidades;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Presentación.Models.Venta.DatalleVenta
{
    public class DetalleVentaViewModel
    {

        [Required]
        [ForeignKey("IdVenta")]
        public int IdVenta { get; set; }
 
        [ForeignKey("IdArticulo")]
        public int IdArticulo { get; set; }
        public string nombreArticulo { get; set; }=string.Empty;
        [Required]
        public int cantidad { get; set; }
        [Required]
        public decimal descuento { get; set; }
        
        public decimal precio { get; set; }


    }
}
