using Microsoft.Build.Framework;
using Presentación.Models.Venta.DetalleIngreos;

namespace Presentación.Models.Venta.Ingresos
{
    public class InsertarIngresoModel
    {
        public int IdPersona { get; set; }
        [Required]
        public int IdUsuario { get; set; }
        [Required]
        public string TipoComprobante { get; set; } = string.Empty;
        public string SerieComprobante { get; set; } = string.Empty;
        [Required]
        public string NumeroComprobante { get; set; } = string.Empty;
        [Required]
        public decimal Impuestos { get; set; }
        [Required]
        public decimal Total { get; set; }
        [Required]
        public List<DetalleIngresosViewModel> Detalles { get; set; }=new List<DetalleIngresosViewModel>();
    }
}
