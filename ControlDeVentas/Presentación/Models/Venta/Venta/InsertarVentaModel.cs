using Entidades;
using Presentación.Models.Venta.DatalleVenta;
using System.ComponentModel.DataAnnotations;

namespace Presentación.Models.Venta.Venta
{
    public class InsertarVentaModel
    {
        [Required]
        public int IdPersona { get; set; }
        [Required]
        public int IdUsuario { get; set;}
        [Required]
        public string TipoComprobante { get; set; } = string.Empty;
        [Required]
        public string SerieComprobante { get; set; } = string.Empty;
        [Required]
        public string NumeroComprobante { get; set; } = string.Empty;
        [Required]
        public decimal Impuesto { get; set; }
        [Required]
        public decimal Total { get; set; }
        [Required]
        public List<DetalleVentaViewModel>Detalles { get; set; }=new List<DetalleVentaViewModel>();

    }
}
