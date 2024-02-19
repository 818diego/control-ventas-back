namespace Presentación.Models.Venta.DatalleVenta
{
    public class ModificarDetalleVentaModel
    {
        public int IdDetalleVentas { get; set; }
        public int IdVenta { get; set; }
        public int IdArticulo { get; set; }
        public int Cantidad { get; set; }
        public decimal Descuento { get; set; }
    }
}
