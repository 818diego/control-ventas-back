namespace Presentación.Models.Venta.DetalleIngreos
{
    public class InsertarDetalleIngresoModel
    {
        public int IdIngreso { get; set; }
        public int IdArticulo { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }
    }
}
