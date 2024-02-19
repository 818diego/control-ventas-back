namespace Presentación.Models.Venta.Ingresos
{
    public class ListandoIngresoViewModel
    {
        public int IdIngreso { get; set; }
        public int IdPersona { get; set; }
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; }=string.Empty;
        public string NombreProvedor { get; set; }=string.Empty;
        public string TipoComprobante { get; set; } = string.Empty;
        public string SerieComprobante { get; set; }=string.Empty;
        public string NumeroComprobante { get; set; } = string.Empty;
        public DateTime FechaHoraIngreso { get; set; } 
        public decimal Impuestos { get; set; }
        public decimal Total { get; set; }
        public bool Estado { get; set; }

    }
}
