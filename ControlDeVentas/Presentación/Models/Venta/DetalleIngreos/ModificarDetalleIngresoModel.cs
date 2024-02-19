namespace Presentación.Models.Venta.DetalleIngreos
{
    public class ModificarDetalleIngresoModel
    {
        public int IdDetalleIngreso { get; set; }
        public int IdIngreso { get; set; }
        public int IdArticulo { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
    }
}
