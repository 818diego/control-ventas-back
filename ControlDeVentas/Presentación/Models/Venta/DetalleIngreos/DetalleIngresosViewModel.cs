using Entidades;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Presentación.Models.Venta.DetalleIngreos
{
    public class DetalleIngresosViewModel
    {
        [Required]
        public int IdIngreso { get; set; }
        public int IdArticulo { get; set; }
        public string NombreArticulo { get; set; }=string.Empty;
        [Required]
        public int Cantidad { get; set; }
        [Required]
        public decimal Precio { get; set; }
    }
}
