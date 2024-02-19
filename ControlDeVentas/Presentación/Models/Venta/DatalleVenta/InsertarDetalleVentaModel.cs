using Entidades;
using System.ComponentModel.DataAnnotations.Schema;

namespace Presentación.Models.Venta.DatalleVenta
{
    public class InsertarDetalleVentaModel
    {
        public int IdVenta { get; set; }
        public int IdArticulo { get; set; }
        public int Cantidad { get; set; }
        public decimal Descuento { get; set; }

    }
}
