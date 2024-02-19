using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Presentación.Models.Venta.Venta
{
    public class VentaViewModel
    {
        public int IdVenta { get; set; }
        public int IdPersona { get; set; }
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; }=string.Empty;
        public string NombreCliente { get; set; }=string.Empty;
        public string TipoComprobante { get; set; } = string.Empty;
        public string SerieComprobante { get; set; } = string.Empty;
        public string NumeroComprobante { get; set; } = string.Empty;
        public DateTime FechaHora { get; set; }
        public decimal Impuesto { get; set; }
        public decimal Total { get; set; }
        public bool Estado { get; set; } = false;
        
    }
}
